namespace DataIngestion.TestAssignment.Entities
{
    public class CollectionMatchImport
    {
        public int? ExportDate { get; set; }

        public long? CollectionId { get; set; }

        public string UPC { get; set; }

        public string Grid { get; set; }

        public int? AmgAlbumId { get; set; }
    }
}