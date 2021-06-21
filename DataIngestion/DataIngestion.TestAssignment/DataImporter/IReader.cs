using System.IO;

namespace DataIngestion.TestAssignment.DataImporter
{
    public interface IReader
    {
        StreamReader GetReader(string path);
    }
}