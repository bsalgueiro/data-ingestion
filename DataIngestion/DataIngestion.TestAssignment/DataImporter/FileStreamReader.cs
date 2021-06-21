using System.IO;

namespace DataIngestion.TestAssignment.DataImporter
{
    public class FileStreamReader : IReader
    {
        public StreamReader GetReader(string path)
        {
            return new StreamReader(path);
        }
    }
}
