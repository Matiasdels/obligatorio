using Microsoft.Extensions.Logging;
using obligatorio.Data;

namespace obligatorio
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Ruta para la base de datos
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "miBase.db3");

            // Registro del servicio con parámetro
            builder.Services.AddSingleton<DataBaseService>(s => new DataBaseService(dbPath));

            return builder.Build();
        }
    }
}