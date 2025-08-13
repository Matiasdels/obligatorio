using obligatorio.Data;

namespace obligatorio
{
    public partial class App : Application
    {

        public static DataBaseService Database { get; private set; }

        public App()
        {
            InitializeComponent();

            //string dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db3");
            //Database = new DataBaseService(dbPath);

            //Database.InitAsync().Wait(); // Crea tabla Sucursal


            MainPage = new AppShell();

            Shell.Current.GoToAsync("//LoginPage");


        }
    }
}
