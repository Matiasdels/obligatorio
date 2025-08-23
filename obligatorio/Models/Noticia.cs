using System.Text.Json.Serialization;

namespace obligatorio.Models
{
    public class Noticia
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("creator")]
        public List<string> Creator { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("pubDate")]
        public string PubDate { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("source_id")]
        public string SourceId { get; set; }

        [JsonPropertyName("category")]
        public List<string> Category { get; set; }

        [JsonPropertyName("country")]
        public List<string> Country { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        // Constructor
        public Noticia()
        {
            Title = "";
            Link = "";
            Creator = new List<string>();
            Description = "";
            PubDate = "";
            ImageUrl = "";
            SourceId = "";
            Category = new List<string>();
            Country = new List<string>();
            Language = "";
        }

        // Propiedad de ayuda para mostrar creadores como string
        public string CreatorString => Creator != null && Creator.Any()
            ? string.Join(", ", Creator)
            : "Autor desconocido";
    }

    public class RespuestaNoticias
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("totalResults")]
        public int TotalResults { get; set; }

        [JsonPropertyName("results")]
        public List<Noticia> Results { get; set; }

        // Constructor
        public RespuestaNoticias()
        {
            Status = "";
            TotalResults = 0;
            Results = new List<Noticia>();
        }
    }
}
