using System;
using Newtonsoft.Json;

namespace obligatorio.Models
{
    // Mantén esta clase para compatibilidad con tu API actual
    public partial class Cine
    {
        [JsonProperty("page")]
        public int Page { get; set; } // Cambié de long a int

        [JsonProperty("results")]
        public Movie[] Results { get; set; } // Cambié Result[] por Movie[]

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; } // Cambié de long a int

        [JsonProperty("total_results")]
        public int TotalResults { get; set; } // Cambié de long a int

        public static Cine FromJson(string json) => JsonConvert.DeserializeObject<Cine>(json, Converter.Settings);
    }

    // 🗑️ ELIMINA la clase Result - ahora usamos Movie directamente
    // La clase Movie ya tiene todas las propiedades con JsonProperty

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}