using obligatorio.Models;

namespace obligatorio;

public partial class PreferenciasPage : ContentPage
{
    private PreferenciasPageViewModel _viewModel;

    public PreferenciasPage()
    {
        InitializeComponent();
        _viewModel = new PreferenciasPageViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Opcional: Recargar preferencias cuando la página aparece
        // _viewModel.RecargarPreferencias(); // Si implementas este método
    }

    // Evento específico para el cambio de tema
    private void OnTemaOscuroToggled(object sender, ToggledEventArgs e)
    {
        if (_viewModel != null)
        {
            // Aplicar el tema inmediatamente
            _viewModel.AplicarTema(e.Value);
        }
    }

    private async void btnAtras_Clicked(object sender, EventArgs e)
    {
        try
        {
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
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}