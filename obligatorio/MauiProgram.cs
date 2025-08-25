using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using obligatorio.Data;
using obligatorio.Models;
using obligatorio.Services;

namespace obligatorio
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Ruta para la base de datos
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "miBase.db3");

            builder.Services.AddSingleton<DataBaseService>(s => new DataBaseService(dbPath));
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ClimaPage>();
            builder.Services.AddSingleton<ClienteListPage>();
            builder.Services.AddSingleton<ClienteDetailPage>();
            builder.Services.AddSingleton<UsuarioService>();
            builder.Services.AddSingleton<IUsuarioService, UsuarioService>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<App>();
            builder.Services.AddTransient<PatrocinadoresPage>();
            builder.Services.AddTransient<MapaPatrocinadorPage>();
            builder.Services.AddTransient<GestionPatrocinadoresPage>();
            builder.Services.AddTransient<VerPatrocinadorPage>();
            builder.Services.AddTransient<BannerUsuarioViewModel>();
            builder.Services.AddTransient<PatrocinadoresViewModel>();
            builder.Services.AddTransient<CrearPatrocinadorPage>();
            builder.Services.AddTransient<EditarPatrocinadorPage>();
            builder.Services.AddSingleton<PreferenciasUsuarioService>();
            builder.Services.AddSingleton<IPreferenciasUsuarioService, PreferenciasUsuarioService>();
            builder.Services.AddTransient<Perfil>();
            builder.Services.AddTransient<PreferenciasPage>();
            builder.Services.AddTransient<CotizacionesPage>();
            builder.Services.AddTransient<NoticiasPage>();
            builder.Services.AddTransient<CinePage>();
            builder.Services.AddTransient<MainPageViewModel>();


            



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}