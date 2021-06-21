using System;
using System.IO;
using System.Linq;
using System.Net;
using DataIngestion.TestAssignment.CollectionManager;
using DataIngestion.TestAssignment.DataImporter;
using DataIngestion.TestAssignment.DataManager;
using DataIngestion.TestAssignment.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;

namespace DataIngestion.TestAssignment
{
    public class Program
	{
        public Program(
            ILogger<Program> logger,
            IConfiguration configuration,
            IDataManager fileManager,
            IDataImporter fileImporter,
            ICollectionManager collectionManager,
            ICollectionsRepository repository)
        {
            _logger = logger;
            _configuration = configuration;
            _fileManager = fileManager;
            _fileImporter = fileImporter;
            _collectionManager = collectionManager;
            _repository = repository;
        }

        private readonly ILogger<Program> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDataManager _fileManager;
        private readonly IDataImporter _fileImporter;
        private readonly ICollectionManager _collectionManager;
        private readonly ICollectionsRepository _repository;

        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<Program>().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            // Adds the configuration before the app build
            // We need it to register the DB client using the appsettings values
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json")
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    builder.Sources.Clear();
                    builder.AddConfiguration(configuration);
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<Program>();
                    services.AddScoped<IDataManager, FileManager>();
                    services.AddScoped<IReader, FileStreamReader>();
                    services.AddScoped<IDataImporter, FileImporter>();
                    services.AddScoped<IWebClient, FileDownloader>();
                    services.AddScoped<ICollectionManager, CollectionManager.CollectionManager>();
                    services.AddSingleton<ICollectionsRepository, ElasticSearchRepository>(serviceProvider => 
                    {
                        var client = serviceProvider.GetService<IElasticClient>();
                        var logger = serviceProvider.GetService<ILogger<ElasticSearchRepository>>();
                        return new ElasticSearchRepository(client, logger);
                    });
                    services.AddElasticSearch(configuration);
                    services.AddLogging(configure => configure.AddConsole());
                });
        }

        public void Run()
        {
            #region Download Files

            if (!Directory.Exists(_configuration["AppPath"] + _configuration["DownloadFolder"]))
            {
                Directory.CreateDirectory(_configuration["AppPath"] + _configuration["DownloadFolder"]);
            }

            string baseUrl = _configuration["GoogleDrive:FileURI"].Replace("APIKEY", _configuration["GoogleDrive:ApiKey"]);

            string artistFileURI = baseUrl.Replace("FILEID", _configuration["GoogleDrive:ArtistFileId"]);
            string artistCollectionFileURI = baseUrl.Replace("FILEID", _configuration["GoogleDrive:ArtistCollectionFileId"]);
            string collectionFileURI = baseUrl.Replace("FILEID", _configuration["GoogleDrive:CollectionFileId"]);
            string collectionMatchFileURI = baseUrl.Replace("FILEID", _configuration["GoogleDrive:CollectionMatchFileId"]);

            _fileManager.GetFile(artistFileURI, _configuration["AppPath"] + _configuration["DownloadFolder"] + _configuration["FileNames:ArtistFileName"] + _configuration["ZipFileExtension"]);
            _fileManager.GetFile(artistCollectionFileURI, _configuration["AppPath"] + _configuration["DownloadFolder"] + _configuration["FileNames:ArtistCollectionFileName"] + _configuration["ZipFileExtension"]);
            _fileManager.GetFile(collectionFileURI, _configuration["AppPath"] + _configuration["DownloadFolder"] + _configuration["FileNames:CollectionFileName"] + _configuration["ZipFileExtension"]);
            _fileManager.GetFile(collectionMatchFileURI, _configuration["AppPath"] + _configuration["DownloadFolder"] + _configuration["FileNames:CollectionMatchFileName"] + _configuration["ZipFileExtension"]);

            #endregion

            #region Extract Files

            _fileManager.ExtractFiles(_configuration["AppPath"] + _configuration["DownloadFolder"], _configuration["ExtractionFolder"], _configuration["FileNames:ArtistFileName"] + _configuration["ZipFileExtension"]);
            _fileManager.ExtractFiles(_configuration["AppPath"] + _configuration["DownloadFolder"], _configuration["ExtractionFolder"], _configuration["FileNames:ArtistCollectionFileName"] + _configuration["ZipFileExtension"]);
            _fileManager.ExtractFiles(_configuration["AppPath"] + _configuration["DownloadFolder"], _configuration["ExtractionFolder"], _configuration["FileNames:CollectionFileName"] + _configuration["ZipFileExtension"]);
            _fileManager.ExtractFiles(_configuration["AppPath"] + _configuration["DownloadFolder"], _configuration["ExtractionFolder"], _configuration["FileNames:CollectionMatchFileName"] + _configuration["ZipFileExtension"]);

            #endregion

            #region Read Files

            var artistsImport = _fileImporter.ImportArtists().ToList();
            var collectionsImport = _fileImporter.ImportCollections().ToList();
            var artistCollectionsImport = _fileImporter.ImportArtistCollections().ToList();
            var collectionMatchesImport = _fileImporter.ImportCollectionMatches().ToList();

            #endregion

            #region Generate objects to be inserted in the ElasticSearch DB

            var collections = _collectionManager.GenerateCollections(artistsImport, collectionsImport, artistCollectionsImport, collectionMatchesImport);

            #endregion

            #region Insert objetcs into ElasticSearch DB

            _repository.DeleteAllEntries();
            _repository.InsertCollections(collections);

            #endregion

            #region Querying database for results

            var searchResponse = _repository.QueryCollections();

            foreach (var c in searchResponse.Documents)
            {
                Console.WriteLine($"{c.Name} | {c.Upc} | {c.ReleaseDate} | {c.Label} | {c.Url}");
            }

            #endregion
        }
    }
}