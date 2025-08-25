using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using obligatorio.Models;
using obligatorio.Services;

namespace obligatorio.ViewModels
{
    public class CinePageViewModel : INotifyPropertyChanged
    {
        private readonly TmdbService _tmdbService; // ✅ Usando TU servicio
        private ObservableCollection<Movie> _movies;
        private ObservableCollection<Movie> _allMovies; // Cache para búsquedas
        private bool _isLoading;
        private bool _isRefreshing;
        private bool _hasError;
        private string _errorMessage;
        private string _emptyStateTitle = "🎬 Películas en Cartelera";
        private string _emptyStateMessage = "Cargando las últimas películas...";

        public CinePageViewModel()
        {
            _tmdbService = new TmdbService(); // ✅ Instanciar tu servicio
            Movies = new ObservableCollection<Movie>();
            _allMovies = new ObservableCollection<Movie>();

            // Comandos
            RefreshCommand = new Command(async () => await RefreshMoviesAsync());
            SearchCommand = new Command<string>(async (query) => await SearchMoviesAsync(query));
            MovieTappedCommand = new Command<Movie>(async (movie) => await OnMovieTapped(movie));

            // ✅ Carga inicial automática
            _ = Task.Run(LoadInitialMoviesAsync);
        }

        #region Properties

        public ObservableCollection<Movie> Movies
        {
            get => _movies;
            set
            {
                _movies = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public bool HasError
        {
            get => _hasError;
            set
            {
                _hasError = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public string EmptyStateTitle
        {
            get => _emptyStateTitle;
            set
            {
                _emptyStateTitle = value;
                OnPropertyChanged();
            }
        }

        public string EmptyStateMessage
        {
            get => _emptyStateMessage;
            set
            {
                _emptyStateMessage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand MovieTappedCommand { get; }

        #endregion

        #region Methods

        private async Task LoadInitialMoviesAsync()
        {
            try
            {
                IsLoading = true;
                HasError = false;

                System.Diagnostics.Debug.WriteLine("🚀 Iniciando carga inicial de películas...");

                // ✅ Usar TU método existente
                var cineResponse = await _tmdbService.GetNowPlayingMoviesAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    UpdateMoviesList(cineResponse);

                    if (!Movies.Any())
                    {
                        EmptyStateTitle = "😔 Sin películas";
                        EmptyStateMessage = "No se encontraron películas en cartelera";
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"✅ {Movies.Count} películas cargadas exitosamente");
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error cargando películas: {ex.Message}");

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    HasError = true;
                    ErrorMessage = ex.Message;
                    EmptyStateTitle = "⚠️ Error de Conexión";
                    EmptyStateMessage = "Verifica tu conexión a internet e intenta nuevamente";
                });
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task RefreshMoviesAsync()
        {
            try
            {
                IsRefreshing = true;
                HasError = false;

                var cineResponse = await _tmdbService.GetNowPlayingMoviesAsync();
                UpdateMoviesList(cineResponse);

                System.Diagnostics.Debug.WriteLine($"🔄 Películas actualizadas: {Movies.Count}");
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"❌ Error refrescando: {ex.Message}");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task SearchMoviesAsync(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    // Restaurar películas originales
                    Movies.Clear();
                    foreach (var movie in _allMovies)
                    {
                        Movies.Add(movie);
                    }
                    return;
                }

                if (query.Length < 2) return; // Esperar al menos 2 caracteres

                System.Diagnostics.Debug.WriteLine($"🔍 Buscando: '{query}'");

                var searchResults = await _tmdbService.SearchMoviesAsync(query);

                Movies.Clear();
                if (searchResults?.Results != null)
                {
                    foreach (var movie in searchResults.Results)
                    {
                        Movies.Add(movie);
                    }
                }

                if (!Movies.Any())
                {
                    EmptyStateTitle = "🔍 Sin Resultados";
                    EmptyStateMessage = $"No se encontraron películas para '{query}'";
                }

                System.Diagnostics.Debug.WriteLine($"🎬 Resultados de búsqueda: {Movies.Count}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en búsqueda: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error",
                    "No se pudo realizar la búsqueda", "OK");
            }
        }

        private async Task OnMovieTapped(Movie movie)
        {
            try
            {
                await Application.Current.MainPage.DisplayAlert(
                    movie.Title,
                    movie.Overview ?? "Sin descripción disponible",
                    "OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error al mostrar detalle: {ex.Message}");
            }
        }

        private void UpdateMoviesList(Cine cineResponse)
        {
            Movies.Clear();
            _allMovies.Clear();

            if (cineResponse?.Results != null)
            {
                foreach (var movie in cineResponse.Results)
                {
                    Movies.Add(movie);
                    _allMovies.Add(movie);
                }
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
