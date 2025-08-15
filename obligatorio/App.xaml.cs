using obligatorio.Data;

namespace obligatorio
{
    public partial class App : Application
    {

        public static DataBaseService Database { get; private set; }

        public App()
        {
            InitializeComponent();

            string dbPath = Path.Combine(
                FileSystem.AppDataDirectory, // Carpeta segura de la app
                "app.db3"                     // Nombre del archivo SQLite
            );

            Database = new DataBaseService(dbPath);



            MainPage = new AppShell();

            Shell.Current.GoToAsync("//LoginPage");


        }
    }
}
