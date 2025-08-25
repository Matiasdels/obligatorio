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
        try
        {
            await Shell.Current.GoToAsync(nameof(CrearPatrocinadorPage));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo navegar: {ex.Message}", "OK");
        }
    }

    private async void OnVerDetallesClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Patrocinador patrocinador)
        {
            try
            {
                // Pasar el objeto completo en lugar de solo el ID
                await Shell.Current.GoToAsync(nameof(VerPatrocinadorPage),
                    new Dictionary<string, object>
                    {
                        { "Patrocinador", patrocinador }
                    });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo abrir los detalles: {ex.Message}", "OK");
            }
        }
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Patrocinador patrocinador)
        {
            try
            {
                // Pasar el objeto completo
                await Shell.Current.GoToAsync(nameof(EditarPatrocinadorPage),
                    new Dictionary<string, object>
                    {
                        { "Patrocinador", patrocinador }
                    });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo abrir la edición: {ex.Message}", "OK");
            }
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
        try
        {
            await _viewModel.LoadPatrocinadoresAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al actualizar: {ex.Message}", "OK");
        }
        finally
        {
            RefreshView.IsRefreshing = false;
        }
    }
}