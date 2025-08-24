using obligatorio;
using obligatorio.Models;

namespace obligatorio
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly MainPageViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();

            System.Diagnostics.Debug.WriteLine($"UsuarioService: {App.UsuarioService != null}");
            System.Diagnostics.Debug.WriteLine($"PreferenciasService: {App.PreferenciasUsuarioService != null}");

            _viewModel = new MainPageViewModel(App.PreferenciasUsuarioService, App.UsuarioService);
            BindingContext = _viewModel;

            System.Diagnostics.Debug.WriteLine($"ViewModel creado: {_viewModel != null}");
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

        private void btnClima_Clicked(object sender, EventArgs e)
        {

        }

        private async void IrAClientes(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ClienteListPage));
        }

        private async void IrADetallesClientes(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ClienteDetailPage));
        }
    }


}
