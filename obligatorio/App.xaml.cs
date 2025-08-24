using obligatorio.Data;
using obligatorio.Services;

namespace obligatorio
{
    public partial class App : Application
    {

        public static DataBaseService Database { get; private set; }
        
        public static IUsuarioService UsuarioService { get; private set; }
        public static IPreferenciasUsuarioService PreferenciasUsuarioService { get; private set; }
        public App()
        {
            InitializeComponent();

            string dbPath = Path.Combine(
                FileSystem.AppDataDirectory, // Carpeta segura de la app
                "app.db3"                     // Nombre del archivo SQLite
            );

           
            Database = new DataBaseService(dbPath);
            UsuarioService = new UsuarioService();
            PreferenciasUsuarioService = new PreferenciasUsuarioService();
            //MainPage = new NavigationPage(new LoginPage(Database));
            MainPage = new AppShell();


        }
    }
}
