using obligatorio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace obligatorio.Services
{
    public class NewsService
    {
        private const string BaseUrl = "https://newsdata.io/api/1/latest";
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions;

        public NewsService()
        {
            _client = new HttpClient();
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<Noticia>> ObtenerNoticiasAsync(string apiKey = "pub_0855a00c231246178b2cb955f6f22c52")
        {
            var url = $"{BaseUrl}?apikey={apiKey}&language=es";
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var datos = await response.Content.ReadFromJsonAsync<RespuestaNoticias>(_jsonOptions);
            return datos?.Results ?? new List<Noticia>();
        }
    }

    public class RespuestaNoticias
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("results")]
        public List<Noticia> Results { get; set; }
    }
}
