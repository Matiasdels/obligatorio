using Newtonsoft.Json;
using obligatorio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obligatorio.Services
{
    internal class WeatherService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "cf26c283759c6dd96366e95634f39056";
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/";

        public WeatherService()
        {
            _httpClient = new HttpClient();
        }

        // 🔹 Método para obtener clima actual en Punta del Este
        public async Task<Root> GetCurrentWeatherAsync()
        {
            string url = $"{BaseUrl}weather?q=Punta%20del%20Este,UY&units=metric&lang=es&appid={ApiKey}";
            var response = await _httpClient.GetStringAsync(url);
            var weatherData = JsonConvert.DeserializeObject<Root>(response);
            return weatherData;
        }
    }
}
