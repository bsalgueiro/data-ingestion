using System;
using System.Collections.Generic;
using DataIngestion.TestAssignment.Entities;
using Microsoft.Extensions.Logging;
using Nest;

namespace DataIngestion.TestAssignment.Repositories
{
    public class ElasticSearchRepository : ICollectionsRepository
    {
        private IElasticClient _client;
        private readonly ILogger<ElasticSearchRepository> _logger;

        public ElasticSearchRepository(IElasticClient client, ILogger<ElasticSearchRepository> logger)
        {
            _client = client;
            _logger = logger;
        }

        public void InsertCollections(IEnumerable<Collection> collections)
        {
            try
            {
                _logger.LogInformation("Inserting objects into ElasticSearch database...");

                int i = 1;

                var bulkAllObservable = _client.BulkAll(collections, b => b
                    .Index("collection")
                    .BackOffTime("30s")
                    .BackOffRetries(2)
                    .RefreshOnCompleted()
                    .MaxDegreeOfParallelism(Environment.ProcessorCount)
                    .Size(1000)
                )
                .Wait(TimeSpan.FromMinutes(15),
                next => {
                    _logger.LogInformation($"Inserting batch #{i} with {next.Items.Count} objects...");
                    i++;
                });

                _logger.LogInformation("Inserting objects completed.");
            }
            catch(Exception e)
            {
                _logger.LogInformation(e.Message);
            }
        }

        public ISearchResponse<Collection> QueryCollections()
        {
            ISearchResponse<Collection> response = null;

            try
            {
                _logger.LogInformation("Querying for objects in the database...");

                response = _client.Search<Collection>(s => s
                    //.From(0)
                    .Size(10) // Querying for just a few results
                    .Query(q => q
                        .MatchAll()
                    //.Match(m => m
                    //    .Field(f => f.Id)
                    //    .Query("1054518778")
                    //)
                    )
                );
            }
            catch(Exception e)
            {
                _logger.LogInformation(e.Message);
            }

            return response;
        }

        public void DeleteAllEntries()
        {
            try
            {
                _logger.LogInformation("Deleting all entries in the collection...");

                _client.DeleteByQuery<Collection>(del => del
                    .Query(q => q.QueryString(qs => qs.Query("*")))
                );

                _logger.LogInformation("All entries in the collection have been deleted.");
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
        }
    }
}
