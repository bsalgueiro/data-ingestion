using System;
using System.Collections.Generic;

namespace DataIngestion.TestAssignment.Entities
{
    /// <summary>
    /// Represents the Collection entity that is added to the repository
    /// </summary>
    public class Collection
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Upc { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public bool? IsCompilation { get; set; }
        public string Label { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<Artist> Artists { get; set; }
    }
}
