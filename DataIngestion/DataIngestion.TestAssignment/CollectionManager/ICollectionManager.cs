using System;
using System.Collections.Generic;
using DataIngestion.TestAssignment.Entities;

namespace DataIngestion.TestAssignment.CollectionManager
{
    public interface ICollectionManager
    {
        public IEnumerable<Collection> GenerateCollections(
            IEnumerable<ArtistImport> artists,
            IEnumerable<CollectionImport> collections,
            IEnumerable<ArtistCollectionImport> artistCollections,
            IEnumerable<CollectionMatchImport> collectionMatches);
    }
}
