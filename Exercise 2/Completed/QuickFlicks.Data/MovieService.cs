using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Net;

namespace QuickFlicks.Data
{
    /// <summary>
    /// Uses the iTunes Search REST API. 
    /// Further details at:
    ///   https://affiliate.itunes.apple.com/resources/documentation/itunes-store-web-service-search-api/
    /// </summary>
    public class MovieService
    {
        private int NumberOfMoviesPerRequest = 25;
        private string RequestUrl = "https://itunes.apple.com/search?term={0}&entity=movie&limit={1}&offset={2}";

        private string GetPath(string search, int pageNo)
        {
            var searchTerm = WebUtility.UrlEncode(search.Trim());
            var offset = (pageNo - 1) * NumberOfMoviesPerRequest;
            return string.Format(RequestUrl, searchTerm, NumberOfMoviesPerRequest, offset);
        }

        public async Task<IReadOnlyList<Movie>> GetMoviesForSearchAsync(string search, int pageNo = 1)
        {
            // Load the data from the remote service
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GetPath(search, pageNo));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(string.Empty).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var results = JsonConvert.DeserializeObject<MovieSearchResponse>(content);
                if (results.ResultCount > 0)
                {
                    return results.Results.Where(item => item.TrackName?.Length > 0).Select(item =>
                        new Movie
                        {
                            ID = item.TrackId,
                            Title = item.TrackName,
                            Genre = item.PrimaryGenreName,
                            ContentAdvisoryRating = item.ContentAdvisoryRating,
                            Description = item.LongDescription,
                            ArtworkUri = item.ArtworkUrl100
                        }).ToList();
                }

                return new List<Movie>();
            }
        }
    }
}