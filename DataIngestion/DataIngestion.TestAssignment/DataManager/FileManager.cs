using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using Microsoft.Extensions.Logging;

namespace DataIngestion.TestAssignment.DataManager
{
    public class FileManager : IDataManager
    {
        private readonly ILogger<FileManager> _logger;
        private readonly IWebClient _webClient;

        public FileManager(ILogger<FileManager> logger, IWebClient webClient)
        {
            _logger = logger;
            _webClient = webClient;
        }

        public void GetFile(string url, string fileName)
        {
            _logger.LogInformation($"Downloading file {fileName}...");

            if (!File.Exists(fileName))
            {
                using (var webClient = _webClient.GetWebClient())
                {
                    try
                    {
                        webClient.DownloadFile(new Uri(url), fileName);
                    }
                    catch (Exception e)
                    {
                        _logger.LogInformation(e.Message);
                    }
                }
            }
        }

        public void ExtractFiles(string basePath, string extractionFolder, string fileName)
        {
            _logger.LogInformation($"Extracting file {fileName} to {basePath}{extractionFolder}...");

            string destinationPath = $"{basePath}{extractionFolder}";

            if(Directory.Exists(basePath))
            {
                string filePath = $"{basePath}{fileName}";
                ZipFile.ExtractToDirectory(filePath, destinationPath, true);
            }
        }
    }
}
