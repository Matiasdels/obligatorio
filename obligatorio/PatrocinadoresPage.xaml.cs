using obligatorio.Data;
using obligatorio.Models;

namespace obligatorio;

public partial class PatrocinadoresPage : ContentPage
{
    private readonly DataBaseService _databaseService;
    private PatrocinadoresViewModel _viewModel;

    public PatrocinadoresPage()
    {
        InitializeComponent();
        _databaseService = App.Database;
        _viewModel = new PatrocinadoresViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadPatrocinadoresAsync();
    }

    private async void OnPatrocinadorTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Patrocinador patrocinador)
        {
            // Animación de toque
            await frame.ScaleTo(0.95, 100, Easing.CubicOut);
            await frame.ScaleTo(1.0, 100, Easing.CubicOut);

            try
            {
                await Shell.Current.GoToAsync(nameof(MapaPatrocinadorPage),
    new Dictionary<string, object>
    {
        { "patrocinadorId", patrocinador.Id }
    });
            }
            catch (Exception ex)
            {
                // Log del error o mostrar mensaje al usuario
                await DisplayAlert("Error", "No se pudo abrir el mapa", "OK");
            }
        }
    }

    private async void OnGestionarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///gestionpatrocinadores");
    }

    private async void OnAgregarPatrocinadorClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///gestionpatrocinadores/agregar");
    }

    private async void OnRefreshing(object sender, EventArgs e)
    {
        await _viewModel.LoadPatrocinadoresAsync();
        RefreshView.IsRefreshing = false;
    }
}
