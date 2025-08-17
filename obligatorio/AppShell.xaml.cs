using obligatorio;
using obligatorio.Models;
using obligatorio.Services;

namespace obligatorio
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();
            
            BindingContext = new BannerUsuarioViewModel(App.UsuarioService);

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(ClimaPage), typeof(ClimaPage));
            Routing.RegisterRoute(nameof(CotizacionesPage), typeof(CotizacionesPage));
            Routing.RegisterRoute(nameof(NoticiasPage), typeof(NoticiasPage));
            Routing.RegisterRoute(nameof(CinePage), typeof(CinePage));
            Routing.RegisterRoute(nameof(PatrocinadoresPage), typeof(PatrocinadoresPage));
            Routing.RegisterRoute(nameof(ClienteListPage), typeof(ClienteListPage));
            Routing.RegisterRoute(nameof(ClienteDetailPage), typeof(ClienteDetailPage));
        }
    }
}
