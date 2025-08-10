using obligatorio.Services;
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace obligatorio
{
    public partial class ClimaPage : ContentPage
    {
        private readonly WeatherService _weatherService;
         
        public ClimaPage()
        {
            InitializeComponent();
            _weatherService = new WeatherService();
            LoadWeather();
        }

        private async void LoadWeather()
        {
            try 
            {
                var data = await _weatherService.GetCurrentWeatherAsync();
                lblCity.Text = data.name;
                lblTemp.Text = $"{data.main.temp}°C";
                lblDescription.Text = data.weather.First().description;
                imgIcon.Source = $"https://openweathermap.org/img/wn/{data.weather.First().icon}@2x.png";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo obtener el clima: {ex.Message}", "OK");
            }
        }

        private void OnActualizarClicked(object sender, EventArgs e)
        {
            LoadWeather();
        }
    }
}