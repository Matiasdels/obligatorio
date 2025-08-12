using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace obligatorio.Models
{
    public class Cotizaciones
    {
       
        
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("terms")]
            public string Terms { get; set; }

            [JsonProperty("privacy")]
            public string Privacy { get; set; }

            [JsonProperty("timestamp")]
            public long Timestamp { get; set; }

            [JsonProperty("source")]
            public string Source { get; set; }

            [JsonProperty("quotes")]
            public Dictionary<string, double> Quotes { get; set; }
        
    }

    
}