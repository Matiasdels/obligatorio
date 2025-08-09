namespace obligatorio;
using obligatorio.Services;
public partial class CinePage : ContentPage
{
    private readonly TmdbService _tmdbService;

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

            MoviesCollection.ItemsSource = cineData.Results;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar las películas: {ex.Message}", "OK");
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