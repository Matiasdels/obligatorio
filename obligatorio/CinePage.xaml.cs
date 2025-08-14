// C�digo completo corregido para CinePage.xaml.cs:

namespace obligatorio;
using obligatorio.Services;
using obligatorio.Models;
using System.Collections.Generic;
using System.Linq;

public partial class CinePage : ContentPage
{
    private readonly TmdbService _tmdbService;
    private List<Result> allMovies = new List<Result>(); // Lista completa de pel�culas

    public CinePage()
    {
        InitializeComponent();
        _tmdbService = new TmdbService();
        LoadMovies();
    }

    private async void LoadMovies()
    {
        try
        {
            var cineData = await _tmdbService.GetNowPlayingMoviesAsync();
            foreach (var movie in cineData.Results)
            {
                movie.PosterPath = $"https://image.tmdb.org/t/p/w500{movie.PosterPath}";
            }

            // Guardar todas las pel�culas para el filtrado
            allMovies = cineData.Results.ToList();
            MoviesCollection.ItemsSource = allMovies;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar las pel�culas: {ex.Message}", "OK");
        }
    }

    // M�todo para manejar la b�squeda
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.ToLower() ?? "";

        if (string.IsNullOrWhiteSpace(searchText))
        {
            // Mostrar todas las pel�culas si no hay b�squeda
            MoviesCollection.ItemsSource = allMovies;
        }
        else
        {
            // Filtrar por t�tulo y descripci�n
            var filteredMovies = allMovies.Where(movie =>
                movie.Title?.ToLower().Contains(searchText) == true ||
                movie.Overview?.ToLower().Contains(searchText) == true
            ).ToList();

            MoviesCollection.ItemsSource = filteredMovies;
        }
    }

    public async void btnCineAtras_Clicked(object sender, EventArgs e)
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