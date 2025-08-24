using obligatorio.Models;

namespace obligatorio;

public partial class PreferenciasPage : ContentPage
{
    private PreferenciasUsuarioPageViewModel _viewModel;

    public PreferenciasPage()
    {
        InitializeComponent();
        _viewModel = new PreferenciasUsuarioPageViewModel(
            App.PreferenciasUsuarioService,
            App.UsuarioService);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InicializarAsync();
    }

    private async void btnVolver_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Asegurar que los cambios se guarden antes de salir
            await _viewModel.GuardarPreferenciasAsync();

            // Navegar de vuelta
            var navStack = Shell.Current.Navigation.NavigationStack;
            if (navStack.Count > 1)
            {
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.GoToAsync("//MainPage");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al volver: {ex.Message}", "OK");
        }
    }
}