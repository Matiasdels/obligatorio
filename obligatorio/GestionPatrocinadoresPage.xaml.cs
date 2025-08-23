using obligatorio.Data;
using obligatorio.Models;

namespace obligatorio;

public partial class GestionPatrocinadoresPage : ContentPage
{
    

    private readonly DataBaseService _databaseService;
    private PatrocinadoresViewModel _viewModel;

    public GestionPatrocinadoresPage()
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

    private async void OnAgregarNuevoClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///gestionpatrocinadores/formulario");
    }

    private async void OnVerDetallesClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Patrocinador patrocinador)
        {
            await Shell.Current.GoToAsync($"///gestionpatrocinadores/detalle?patrocinadorId={patrocinador.Id}");
        }
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Patrocinador patrocinador)
        {
            await Shell.Current.GoToAsync($"///gestionpatrocinadores/formulario?patrocinadorId={patrocinador.Id}");
        }
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Patrocinador patrocinador)
        {
            var result = await DisplayAlert(
                "Confirmar eliminación",
                $"¿Estás seguro de que quieres eliminar a '{patrocinador.Nombre}'?",
                "Eliminar",
                "Cancelar");

            if (result)
            {
                try
                {
                    await _databaseService.DeletePatrocinadorAsync(patrocinador);
                    await _viewModel.LoadPatrocinadoresAsync();

                    await DisplayAlert("Éxito",
                        $"'{patrocinador.Nombre}' ha sido eliminado correctamente.",
                        "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error",
                        $"No se pudo eliminar el patrocinador: {ex.Message}",
                        "OK");
                }
            }
        }
    }

    private async void OnRefreshing(object sender, EventArgs e)
    {
        await _viewModel.LoadPatrocinadoresAsync();
        RefreshView.IsRefreshing = false;
    }
}