using System;
using System.Net.Http;
using System.Threading.Tasks;
using obligatorio.Models;

namespace obligatorio.Services
{
    public class TmdbService
    {
        private const string ApiKey = "aba18bfa1439c7faa68dbd95b1fda8d5"; // ✅ Tu API Key funciona
        private const string BaseUrl = "https://api.themoviedb.org/3/";
        private readonly HttpClient _httpClient;

        public TmdbService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30); // Añadir timeout
        }

        // ✅ Tu método existente - perfecto
        public async Task<Cine> GetNowPlayingMoviesAsync()
        {
            try
            {
                string url = $"{BaseUrl}movie/now_playing?api_key={ApiKey}&language=es-ES&page=1";

                System.Diagnostics.Debug.WriteLine($"🌐 Llamando: {url}");

                var json = await _httpClient.GetStringAsync(url);

                System.Diagnostics.Debug.WriteLine($"📄 Respuesta recibida: {json.Length} caracteres");

                var result = Cine.FromJson(json);

                System.Diagnostics.Debug.WriteLine($"🎬 Películas parseadas: {result?.Results?.Length ?? 0}");

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en GetNowPlayingMoviesAsync: {ex.Message}");
                throw;
            }
        }

        // ✅ Añadir método para películas populares (para tener más opciones)
        public async Task<Cine> GetPopularMoviesAsync(int page = 1)
        {
            try
            {
                string url = $"{BaseUrl}movie/popular?api_key={ApiKey}&language=es-ES&page={page}";

                System.Diagnostics.Debug.WriteLine($"🌐 Llamando: {url}");

                var json = await _httpClient.GetStringAsync(url);
                var result = Cine.FromJson(json);

                System.Diagnostics.Debug.WriteLine($"🎬 Películas populares: {result?.Results?.Length ?? 0}");

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en GetPopularMoviesAsync: {ex.Message}");
                throw;
            }
        }

        // ✅ Añadir método de búsqueda
        public async Task<Cine> SearchMoviesAsync(string query, int page = 1)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    return await GetNowPlayingMoviesAsync(); // Fallback a películas en cartelera

                var encodedQuery = Uri.EscapeDataString(query.Trim());
                string url = $"{BaseUrl}search/movie?api_key={ApiKey}&language=es-ES&query={encodedQuery}&page={page}";

                System.Diagnostics.Debug.WriteLine($"🔍 Buscando: {url}");

                var json = await _httpClient.GetStringAsync(url);
                var result = Cine.FromJson(json);

                System.Diagnostics.Debug.WriteLine($"🎬 Resultados de búsqueda: {result?.Results?.Length ?? 0}");

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en SearchMoviesAsync: {ex.Message}");
                throw;
            }
        }

        // ✅ Cleanup resources
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
