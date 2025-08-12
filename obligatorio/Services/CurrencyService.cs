using Newtonsoft.Json;
using obligatorio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obligatorio.Services
{
    public class CurrencyService
    {
        private const string ApiKey = "bcaece1a6d1653b96862bea2ac835167"; // Reemplaza con tu clave de CurrencyLayer
        private const string Url = "http://api.currencylayer.com/live";

        public async Task<Cotizaciones> ObtenerCotizacionesAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"{Url}?access_key={ApiKey}&currencies=UYU,EUR,BRL&format=1");

                if (!response.IsSuccessStatusCode)
                    throw new Exception("No se pudo conectar con el servidor de CurrencyLayer");

                var json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Cotizaciones>(json);
            }
        }


    }
}
