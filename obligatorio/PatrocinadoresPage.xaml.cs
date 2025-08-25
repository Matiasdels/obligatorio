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

    private async void OnHeaderPointerEntered(object sender, PointerEventArgs e)
    {
        if (sender is Frame frame)
        {
            await frame.ScaleTo(1.05, 200, Easing.CubicOut);
        }
    }


    private async void OnHeaderPointerExited(object sender, PointerEventArgs e)
    {
        if (sender is Frame frame)
        {
            await frame.ScaleTo(1.0, 200, Easing.CubicOut);
        }
    }


    private async void OnPatrocinadorPointerEntered(object sender, PointerEventArgs e)
    {
        if (sender is Frame frame)
        {
            await Task.WhenAll(
                frame.ScaleTo(1.08, 200, Easing.CubicOut),
                frame.TranslateTo(0, -5, 200, Easing.CubicOut)
            );
        }
    }

    private async void OnPatrocinadorPointerExited(object sender, PointerEventArgs e)
    {
        if (sender is Frame frame)
        {
            await Task.WhenAll(
                frame.ScaleTo(1.0, 200, Easing.CubicOut),
                frame.TranslateTo(0, 0, 200, Easing.CubicOut)
            );
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
