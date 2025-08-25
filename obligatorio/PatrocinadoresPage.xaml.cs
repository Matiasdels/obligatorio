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
            // Animación de toque mejorada
            await AnimateCardTap(frame);

            try
            {
                // Navegar al mapa del patrocinador usando el objeto completo
                await Shell.Current.GoToAsync(nameof(MapaPatrocinadorPage),
                    new Dictionary<string, object>
                    {
                        { "Patrocinador", patrocinador }
                    });
            }
            catch (Exception ex)
            {
                // Log del error y mostrar mensaje al usuario
                System.Diagnostics.Debug.WriteLine($"Error navegando al mapa: {ex.Message}");
                await DisplayAlert("Error", "No se pudo abrir el mapa del patrocinador", "OK");
            }
        }
    }

    private async Task AnimateCardTap(Frame frame)
    {
        try
        {
            // Animación de "press" más suave y profesional
            var scaleDown = frame.ScaleTo(0.95, 100, Easing.CubicOut);
            var fadeOut = frame.FadeTo(0.8, 100, Easing.CubicOut);

            await Task.WhenAll(scaleDown, fadeOut);

            var scaleUp = frame.ScaleTo(1.0, 150, Easing.CubicOut);
            var fadeIn = frame.FadeTo(1.0, 150, Easing.CubicOut);

            await Task.WhenAll(scaleUp, fadeIn);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error en animación: {ex.Message}");
            // Restaurar estado normal en caso de error
            frame.Scale = 1.0;
            frame.Opacity = 1.0;
        }
    }

    private async void OnGestionarClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(GestionPatrocinadoresPage));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error navegando a gestión: {ex.Message}");
            await DisplayAlert("Error", "No se pudo abrir la página de gestión", "OK");
        }
    }

    private async void OnAgregarPatrocinadorClicked(object sender, EventArgs e)
    {
        try
        {
            // Animación del botón flotante
            await BtnAdd.ScaleTo(0.9, 100, Easing.CubicOut);
            await BtnAdd.ScaleTo(1.0, 100, Easing.CubicOut);

            await Shell.Current.GoToAsync(nameof(CrearPatrocinadorPage));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error navegando a crear patrocinador: {ex.Message}");
            await DisplayAlert("Error", "No se pudo abrir la página de creación", "OK");
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
            System.Diagnostics.Debug.WriteLine($"Error al refrescar: {ex.Message}");
            await DisplayAlert("Error", "No se pudieron cargar los patrocinadores", "OK");
        }
        finally
        {
            RefreshView.IsRefreshing = false;
        }
    }

    // Método adicional para manejar cambios de orientación
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        // Ajustar el número de columnas según el ancho de pantalla
        if (PatrocinadoresCollection?.ItemsLayout is GridItemsLayout gridLayout)
        {
            // Para pantallas más grandes, mostrar más columnas
            if (width > 768) // Tablet landscape
            {
                gridLayout.Span = 3;
            }
            else if (width > 480) // Tablet portrait o teléfono landscape
            {
                gridLayout.Span = 2;
            }
            else // Teléfono portrait
            {
                gridLayout.Span = 2; // Mantener 2 columnas para mejor UX
            }
        }
    }
}