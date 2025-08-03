namespace obligatorio
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
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
    }


}
