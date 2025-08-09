using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using obligatorio.Models;


namespace obligatorio.Services
{
    public class TmdbService
    {
        private const string ApiKey = "aba18bfa1439c7faa68dbd95b1fda8d5";
        private const string BaseUrl = "https://api.themoviedb.org/3/";

        private readonly HttpClient _httpClient;

        public TmdbService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Cine> GetNowPlayingMoviesAsync()
        {
            string url = $"{BaseUrl}movie/now_playing?api_key={ApiKey}&language=es-ES&page=1";

            var json = await _httpClient.GetStringAsync(url);

            // Usamos el método que generaste con QuickType
            return Cine.FromJson(json);
        }
    }
}
