using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obligatorio.Models
{
    public class ForecastResponse
    {
        public City city { get; set; }
        public List<ForecastItem> list { get; set; }
    }

    public class City
    {
        public string name { get; set; }
        public string country { get; set; }
    }

    public class ForecastItem
    {
        public MainInfo main { get; set; }
        public List<WeatherInfo> weather { get; set; }
        public DateTime dt_txt { get; set; }
    }

    public class MainInfo
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }

    public class WeatherInfo
    {
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }
}
