namespace obligatorio
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(ClimaPage), typeof(ClimaPage));
            Routing.RegisterRoute(nameof(CotizacionesPage), typeof(CotizacionesPage));
            Routing.RegisterRoute(nameof(NoticiasPage), typeof(NoticiasPage));
            Routing.RegisterRoute(nameof(CinePage), typeof(CinePage));
            Routing.RegisterRoute(nameof(PatrocinadoresPage), typeof(PatrocinadoresPage));
        }
    }
}
