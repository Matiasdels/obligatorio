using obligatorio.Clientes;

namespace obligatorio
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
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
