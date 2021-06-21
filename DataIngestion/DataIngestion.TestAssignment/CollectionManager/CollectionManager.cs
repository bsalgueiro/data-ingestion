using System;
using System.Collections.Generic;
using System.Linq;
using DataIngestion.TestAssignment.Entities;
using Microsoft.Extensions.Logging;

namespace DataIngestion.TestAssignment.CollectionManager
{
    public class CollectionManager : ICollectionManager
    {
        private readonly ILogger<CollectionManager> _logger;

        public CollectionManager(ILogger<CollectionManager> logger)
        {
            _logger = logger;
        }

        public IEnumerable<Collection> GenerateCollections(IEnumerable<ArtistImport> artists, IEnumerable<CollectionImport> collections, IEnumerable<ArtistCollectionImport> artistCollections, IEnumerable<CollectionMatchImport> collectionMatches)
        {
            _logger.LogInformation("Merging data into objets to insert into database...");

            var collectionList = from artistcollection in artistCollections
                              join artist in artists on artistcollection.ArtistId equals artist.Id into artistsInCollection
                              join collection in collections on artistcollection.CollectionId equals collection.Id
                              join collectionMatch in collectionMatches on artistcollection.CollectionId equals collectionMatch.CollectionId

                              select new Collection
                              {
                                  Id = collection.Id.ToString(),
                                  Url = collection.ViewUrl,
                                  ImageUrl = collection.ArtworkUrl,
                                  Label = collection.LabelStudio,
                                  Name = collection.Name,
                                  ReleaseDate = collection.OriginalReleaseDate,
                                  IsCompilation = collection.IsCompilation,
                                  Upc = collectionMatch.UPC,
                                  Artists = artistsInCollection.ToArtistList()
                              };

            _logger.LogInformation($"{collections.Count()} objects were created.");

            return collectionList;
        }
    }
}
