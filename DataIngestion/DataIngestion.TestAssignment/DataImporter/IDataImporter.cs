using System.Collections.Generic;
using DataIngestion.TestAssignment.Entities;

namespace DataIngestion.TestAssignment.DataImporter
{
    public interface IDataImporter
    {
        public IEnumerable<ArtistImport> ImportArtists();

        public IEnumerable<CollectionImport> ImportCollections();

        public IEnumerable<ArtistCollectionImport> ImportArtistCollections();

        public IEnumerable<CollectionMatchImport> ImportCollectionMatches();
    }
}