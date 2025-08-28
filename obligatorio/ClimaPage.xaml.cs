using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using obligatorio.Models;
using obligatorio.Services;
using System;
using System.Linq;

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

        public async Task<ForecastResponse> GetForecastAsync(string city = "Maldonado")
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid=TU_API_KEY&units=metric&lang=es");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ForecastResponse>(json);
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

                var forecast = await _weatherService.GetForecastAsync();

                var daily = forecast.list
                    .Where(f => f.dt_txt.Hour == 12)
                    .Take(5)
                    .Select(f => new
                    {
                        Date = f.dt_txt.ToString("ddd dd"),
                        Temp = $"{f.main.temp}°C",
                        Description = f.weather.First().description,
                        Icon = $"https://openweathermap.org/img/wn/{f.weather.First().icon}@2x.png"
                    });

                forecastCollection.ItemsSource = daily.ToList();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo obtener el clima: {ex.Message}", "OK");
            }
        }
        private async void OnHeaderPointerEntered(object sender, PointerEventArgs e)
        {
            if (sender is Frame frame)
            {
                await frame.ScaleTo(1.05, 200, Easing.CubicOut);
            }
        }

  
        private async void OnHeaderPointerExited(object sender, PointerEventArgs e)
        {
            if (sender is Frame frame)
            {
                await frame.ScaleTo(1.0, 200, Easing.CubicOut);
            }
        }


        private async void OnClimaPointerEntered(object sender, PointerEventArgs e)
        {
            if (sender is Frame frame)
            {
                await Task.WhenAll(
                    frame.ScaleTo(1.08, 200, Easing.CubicOut),
                    frame.TranslateTo(0, -5, 200, Easing.CubicOut)
                );
            }
        }

        private async void OnClimaPointerExited(object sender, PointerEventArgs e)
        {
            if (sender is Frame frame)
            {
                await Task.WhenAll(
                    frame.ScaleTo(1.0, 200, Easing.CubicOut),
                    frame.TranslateTo(0, 0, 200, Easing.CubicOut)
                );
            }
        }
        private async void btnAtras_Clicked(object sender, EventArgs e)
        {
            try
            {
                var navStack = Shell.Current.Navigation.NavigationStack;
                if (navStack.Count > 1)
                {
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.GoToAsync("//MainPage");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}