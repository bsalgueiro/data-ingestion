using System;
using System.Collections.Generic;
using DataIngestion.TestAssignment.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace DataIngestion.TestAssignment
{
    public static class Extensions
    {
        /// <summary>
        /// Converts an IEnumerable<ArtistImport> to List<Artist>
        /// </summary>
        /// <param name="artistImportList"></param>
        /// <returns>List<Artist></returns>
        public static List<Artist> ToArtistList(this IEnumerable<ArtistImport> artistImportList)
        {
            var artistList = new List<Artist>();

            foreach(var artist in artistImportList)
            {
                artistList.Add(new Artist
                {
                    Id = artist.Id.ToString(),
                    Name = artist.Name
                });
            }

            return artistList;
        }

        /// <summary>
        /// Registers the ElasticSearch as a service using the app settings
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ElasticSearch:url"];
            var defaultIndex = configuration["ElasticSearch:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }
    }
}