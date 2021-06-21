using System;
using System.IO;

namespace DataIngestion.TestAssignment.DataManager
{
    public interface IDataManager
    {
        public void GetFile(string url, string fileName);

        public void ExtractFiles(string targetPath, string extractionFolder, string fileName);
    }
}
