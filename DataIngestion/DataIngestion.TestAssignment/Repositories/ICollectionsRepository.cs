using System.Collections.Generic;
using DataIngestion.TestAssignment.Entities;
using Nest;

namespace DataIngestion.TestAssignment.Repositories
{
    public interface ICollectionsRepository
    {
        public void InsertCollections(IEnumerable<Collection> collections);
        
        public ISearchResponse<Collection> QueryCollections();

        public void DeleteAllEntries();
    }
}
