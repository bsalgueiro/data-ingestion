using System;
using System.Collections.Generic;
using System.IO;
using DataIngestion.TestAssignment.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataIngestion.TestAssignment.DataImporter
{
    public class FileImporter : IDataImporter
    {
        private readonly string LineSeparator = "\u0001";
        private readonly string LineEnding = "\u0002";
        private readonly string EscapeCharacter = "#";

        private readonly IConfiguration _configuration;
        private readonly ILogger<FileImporter> _logger;
        private readonly IReader _reader;

        public FileImporter(IConfiguration configuration, ILogger<FileImporter> logger, IReader reader)
        {
            _configuration = configuration;
            _logger = logger;
            _reader = reader;
        }

        public IEnumerable<ArtistImport> ImportArtists()
        {
            _logger.LogInformation($"Importing data from file '{_configuration["FileNames:ArtistFileName"]}'...");

            var artists = new List<ArtistImport>();

            try
            {
                using (var reader = _reader.GetReader(
                    $"{_configuration["AppPath"]}" +
                    $"{_configuration["DownloadFolder"]}" +
                    $"{_configuration["ExtractionFolder"]}" +
                    $"{_configuration["FileNames:ArtistFileName"]}"))
                {
                    string data;

                    while (!reader.EndOfStream)
                    {
                        // Skip headers
                        data = reader.ReadLine();
                        if (data.StartsWith(EscapeCharacter)) continue;

                        // Read lines until find the line-ending character
                        while (!data.EndsWith(LineEnding))
                            data += reader.ReadLine();

                        // Remove Line ending character
                        data = data.Replace(LineEnding, "");

                        // Split values into array of strings
                        var values = data.Split(LineSeparator);

                        values = Helpers.ReplaceEmptyStringsWithNull(values);

                        artists.Add(new ArtistImport
                        {
                            ExportDate = Convert.ToInt32(values[0]),
                            Id = Convert.ToInt32(values[1]),
                            Name = values[2],
                            IsActualArtist = values[3] == "1" ? true : false,
                            ViewUrl = values[4],
                            ArtistTypeId = Convert.ToInt32(values[5])
                        });
                    }

                    _logger.LogInformation($"{artists.Count} rows imported from file '{_configuration["FileNames:ArtistFileName"]}'.");
                }
            }
            catch(Exception e)
            {
                _logger.LogInformation(e.Message);
            }
            
            return artists;
        }

        public IEnumerable<CollectionImport> ImportCollections()
        {
            _logger.LogInformation($"Importing data from file '{_configuration["FileNames:CollectionFileName"]}'...");

            var collections = new List<CollectionImport>();

            try
            {
                using (var reader = new StreamReader(
                    $"{_configuration["AppPath"]}" +
                    $"{_configuration["DownloadFolder"]}" +
                    $"{_configuration["ExtractionFolder"]}" +
                    $"{_configuration["FileNames:CollectionFileName"]}"))
                {
                    string data;

                    while (!reader.EndOfStream)
                    {
                        // Skip headers
                        data = reader.ReadLine();
                        if (data.StartsWith(EscapeCharacter)) continue;

                        // Read lines until find the line-ending character
                        while (!data.EndsWith(LineEnding))
                            data += reader.ReadLine();

                        // Remove Line ending character
                        data = data.Replace(LineEnding, "");

                        // Split values into array of strings
                        var values = data.Split(LineSeparator);

                        values = Helpers.ReplaceEmptyStringsWithNull(values);

                        collections.Add(new CollectionImport
                        {
                            ExportDate = Convert.ToInt32(values[0]),
                            Id = Convert.ToInt64(values[1]),
                            Name = values[2],
                            TitleVersion = values[3],
                            SearchTerms = values[4],
                            ParentalAdvisoryId = Convert.ToInt32(values[5]),
                            ArtistDisplayName = values[6],
                            ViewUrl = values[7],
                            ArtworkUrl = values[8],
                            OriginalReleaseDate = Convert.ToDateTime(values[9]),
                            ItunesReleaseDate = Convert.ToDateTime(values[10]),
                            LabelStudio = values[11],
                            ContentProviderName = values[12],
                            Copyright = values[13],
                            PLine = values[14],
                            MediaTypeId = Convert.ToInt32(values[15]),
                            IsCompilation = values[16] == "1" ? true : false,
                            CollectionTypeId = values[17]
                        });
                    }

                    _logger.LogInformation($"{collections.Count} rows imported from file '{_configuration["FileNames:CollectionFileName"]}'.");
                }

            }
            catch(Exception e)
            {
                _logger.LogInformation(e.Message);
            }

            return collections;
        }

        public IEnumerable<ArtistCollectionImport> ImportArtistCollections()
        {
            _logger.LogInformation($"Importing data from file '{_configuration["FileNames:ArtistCollectionFileName"]}'...");

            var artistCollection = new List<ArtistCollectionImport>();

            try
            {
                using (var reader = new StreamReader(
                    $"{_configuration["AppPath"]}" +
                    $"{_configuration["DownloadFolder"]}" +
                    $"{_configuration["ExtractionFolder"]}" +
                    $"{_configuration["FileNames:ArtistCollectionFileName"]}"))
                {
                    string data;

                    while (!reader.EndOfStream)
                    {
                        // Skip headers
                        data = reader.ReadLine();
                        if (data.StartsWith(EscapeCharacter)) continue;

                        // Read lines until find the line-ending character
                        while (!data.EndsWith(LineEnding))
                            data += reader.ReadLine();

                        // Remove Line ending character
                        data = data.Replace(LineEnding, "");

                        // Split values into array of strings
                        var values = data.Split(LineSeparator);

                        values = Helpers.ReplaceEmptyStringsWithNull(values);

                        artistCollection.Add(new ArtistCollectionImport
                        {
                            ExportDate = Convert.ToInt32(values[0]),
                            ArtistId = Convert.ToInt32(values[1]),
                            CollectionId = Convert.ToInt64(values[2]),
                            IsPrimaryArtist = values[3] == "1" ? true : false,
                            RoleId = Convert.ToInt32(values[4])
                        });
                    }

                    _logger.LogInformation($"{artistCollection.Count} rows imported from file '{_configuration["FileNames:ArtistCollectionFileName"]}'.");
                }

            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }

            return artistCollection;
        }

        public IEnumerable<CollectionMatchImport> ImportCollectionMatches()
        {
            _logger.LogInformation($"Importing data from file '{_configuration["FileNames:CollectionMatchFileName"]}'...");

            var collectionMatch = new List<CollectionMatchImport>();

            try
            {
                using (var reader = new StreamReader(
                    $"{_configuration["AppPath"]}" +
                    $"{_configuration["DownloadFolder"]}" +
                    $"{_configuration["ExtractionFolder"]}" +
                    $"{_configuration["FileNames:CollectionMatchFileName"]}"))
                {
                    string data;

                    while (!reader.EndOfStream)
                    {
                        // Skip headers
                        data = reader.ReadLine();
                        if (data.StartsWith(EscapeCharacter)) continue;

                        // Read lines until find the line-ending character
                        while (!data.EndsWith(LineEnding))
                            data += reader.ReadLine();

                        // Remove Line ending character
                        data = data.Replace(LineEnding, "");

                        // Split values into array of strings
                        var values = data.Split(LineSeparator);

                        values = Helpers.ReplaceEmptyStringsWithNull(values);

                        collectionMatch.Add(new CollectionMatchImport
                        {
                            ExportDate = Convert.ToInt32(values[0]),
                            CollectionId = Convert.ToInt32(values[1]),
                            UPC = values[2],
                            Grid = values[3],
                            AmgAlbumId = Convert.ToInt32(values[4])
                        });
                    }

                    _logger.LogInformation($"{collectionMatch.Count} records imported from file '{_configuration["FileNames:CollectionMatchFileName"]}'.");
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }

            return collectionMatch;
        }
    }
}