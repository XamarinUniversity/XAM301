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
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ArtworkUri { get; set; }
        public string Genre { get; set; }
        public string ContentAdvisoryRating { get; set; }
        public string Description { get; set; }
    }
}