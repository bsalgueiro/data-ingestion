namespace DataIngestion.TestAssignment.Entities
{
    public class ArtistCollectionImport
    {

        public int? ExportDate { get; set; }

        public int? ArtistId { get; set; }

        public long? CollectionId { get; set; }

        public bool? IsPrimaryArtist { get; set; }

        public int? RoleId { get; set; }
    }
}