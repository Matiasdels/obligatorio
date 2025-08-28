using System;
using Newtonsoft.Json;

namespace obligatorio.Models
{
    public partial class Cine
    {
        [JsonProperty("page")]
        public int Page { get; set; } 

        [JsonProperty("results")]
        public Movie[] Results { get; set; } 

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; } 
        [JsonProperty("total_results")]
        public int TotalResults { get; set; } 

        public static Cine FromJson(string json) => JsonConvert.DeserializeObject<Cine>(json, Converter.Settings);
    }


    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}