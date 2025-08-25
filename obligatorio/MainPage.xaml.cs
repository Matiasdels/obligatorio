using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using obligatorio;
using obligatorio.Models;

namespace obligatorio
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _viewModel;
        private bool isPlaying = false;

        public MainPage()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine($"UsuarioService: {App.UsuarioService != null}");
            System.Diagnostics.Debug.WriteLine($"PreferenciasService: {App.PreferenciasUsuarioService != null}");
            _viewModel = new MainPageViewModel(App.PreferenciasUsuarioService, App.UsuarioService);
            BindingContext = _viewModel;
            System.Diagnostics.Debug.WriteLine($"ViewModel creado: {_viewModel != null}");

            VolumeSlider.Value = 0.7;
            radioPlayer.Volume = 0.7;
        }

        private async void TogglePlayPause(object sender, EventArgs e)
        {
            try
            {
                if (!isPlaying)
                {
                    StatusLabel.Text = "Conectando...";
                    PlayPauseButton.Text = "⏳";
                    PlayPauseButton.IsEnabled = false;

                    await Task.Delay(300);
                    radioPlayer.Play();
                }
                else
                {
                    radioPlayer.Pause();
                    isPlaying = false;
                    PlayPauseButton.Text = "▶️";
                    StatusLabel.Text = "Pausado";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No se pudo conectar a la radio", "OK");
                PlayPauseButton.Text = "▶️";
                PlayPauseButton.IsEnabled = true;
                StatusLabel.Text = "Error de conexión";
            }
        }

        private void OnMediaStateChanged(object sender, MediaStateChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                switch (e.NewState)
                {
                    case MediaElementState.Playing:
                        isPlaying = true;
                        PlayPauseButton.Text = "⏸️";
                        PlayPauseButton.IsEnabled = true;
                        StatusLabel.Text = "Reproduciendo";
                        break;

                    case MediaElementState.Paused:
                    case MediaElementState.Stopped:
                        isPlaying = false;
                        PlayPauseButton.Text = "▶️";
                        PlayPauseButton.IsEnabled = true;
                        StatusLabel.Text = "Detenido";
                        break;

                    case MediaElementState.Failed:
                        isPlaying = false;
                        PlayPauseButton.Text = "▶️";
                        PlayPauseButton.IsEnabled = true;
                        StatusLabel.Text = "Error";
                        break;
                }
            });
        }

        private void OnVolumeChanged(object sender, ValueChangedEventArgs e)
        {
            double volume = e.NewValue;
            radioPlayer.Volume = volume;
            VolumeLabel.Text = $"{(int)(volume * 100)}%";
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            System.Diagnostics.Debug.WriteLine($"OnAppearing - Usuario logueado: {App.UsuarioService?.EstaLogueado}");

            if (App.UsuarioService?.EstaLogueado == true)
            {
                System.Diagnostics.Debug.WriteLine($"Usuario ID: {App.UsuarioService.UsuarioActual?.Id}");
                await _viewModel.InicializarAsync();

                // Debug: Verificar las preferencias después de inicializar
                if (_viewModel.PreferenciasActuales != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Preferencias cargadas:");
                    System.Diagnostics.Debug.WriteLine($"  MostrarClima: {_viewModel.PreferenciasActuales.MostrarClima}");
                    System.Diagnostics.Debug.WriteLine($"  MostrarCotizaciones: {_viewModel.PreferenciasActuales.MostrarCotizaciones}");
                    System.Diagnostics.Debug.WriteLine($"  MostrarNoticias: {_viewModel.PreferenciasActuales.MostrarNoticias}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("PreferenciasActuales es null");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No hay usuario logueado");
            }
        }

        private async void IrAClima(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ClimaPage));
        }

        private async void IrACotizaciones(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CotizacionesPage));
        }

        private async void IrANoticias(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NoticiasPage));
        }

        private async void IrACine(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CinePage));
        }

        private async void IrAPatrocinadores(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(PatrocinadoresPage));
        }

        private async void IrAClientes(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ClienteListPage));
        }

    }


}
