namespace DataIngestion.TestAssignment.Entities
{
    public class ArtistImport
    {
        public int? ExportDate { get; set; }

        public int? Id { get; set; }

        public string Name { get; set; }

        public bool? IsActualArtist { get; set; }

        public string ViewUrl { get; set; }

        public int? ArtistTypeId { get; set; }
    }
}