using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using obligatorio.Models;

namespace obligatorio;

[QueryProperty(nameof(Patrocinador), "Patrocinador")]
public partial class VerPatrocinadorPage : ContentPage
{
    private Patrocinador _patrocinador;

    public Patrocinador Patrocinador
    {
        get => _patrocinador;
        set
        {
            _patrocinador = value;
            OnPropertyChanged();
            if (_patrocinador != null)
            {
                _ = CargarDatosPatrocinadorAsync();
            }
        }
    }

    public VerPatrocinadorPage()
    {
        InitializeComponent();
    }

    private async Task CargarDatosPatrocinadorAsync()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            // Cargar datos b�sicos
            NombreLabel.Text = _patrocinador.Nombre;
            IdLabel.Text = $"#{_patrocinador.Id}";
            CoordenadasLabel.Text = $"Lat: {_patrocinador.Latitud:F6}, Lng: {_patrocinador.Longitud:F6}";

            // Cargar direcci�n si existe
            if (!string.IsNullOrWhiteSpace(_patrocinador.Direccion))
            {
                DireccionLabel.Text = _patrocinador.Direccion;
            }
            else
            {
                DireccionStack.IsVisible = false;
            }

            // Cargar imagen si existe
            if (!string.IsNullOrWhiteSpace(_patrocinador.Imagen))
            {
                try
                {
                    ImagenPatrocinador.Source = _patrocinador.Imagen;
                }
                catch
                {
                    // Si la imagen no se puede cargar, mostrar placeholder
                    ImagenPatrocinador.Source = null;
                }
            }
            else
            {
                // Mostrar placeholder o imagen por defecto
                ImagenPatrocinador.Source = null;
            }

            // Inicializar mapa
            InicializarMapa();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar los datos: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private void InicializarMapa()
    {
        if (_patrocinador == null) return;

        var ubicacion = new Location(_patrocinador.Latitud, _patrocinador.Longitud);

        // Configurar el pin
        PinPatrocinador.Location = ubicacion;
        PinPatrocinador.Label = _patrocinador.Nombre;
        PinPatrocinador.Address = _patrocinador.Direccion ?? "Ubicaci�n del patrocinador";
        PinPatrocinador.Type = PinType.Place;

        // Centrar el mapa en la ubicaci�n
        MapaVisualizacion.MoveToRegion(MapSpan.FromCenterAndRadius(ubicacion, Distance.FromKilometers(1)));
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(EditarPatrocinadorPage),
                new Dictionary<string, object>
                {
                    { "Patrocinador", _patrocinador }
                });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al abrir la p�gina de edici�n: {ex.Message}", "OK");
        }
    }

    private async void OnCompartirUbicacionClicked(object sender, EventArgs e)
    {
        try
        {
            var ubicacionTexto = $"{_patrocinador.Nombre}\n" +
                               $"Ubicaci�n: {_patrocinador.Latitud:F6}, {_patrocinador.Longitud:F6}";

            if (!string.IsNullOrWhiteSpace(_patrocinador.Direccion))
            {
                ubicacionTexto += $"\nDirecci�n: {_patrocinador.Direccion}";
            }

            // URL de Google Maps
            var googleMapsUrl = $"https://www.google.com/maps?q={_patrocinador.Latitud},{_patrocinador.Longitud}";
            ubicacionTexto += $"\nVer en Google Maps: {googleMapsUrl}";

            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Text = ubicacionTexto,
                Title = $"Ubicaci�n de {_patrocinador.Nombre}"
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al compartir la ubicaci�n: {ex.Message}", "OK");
        }
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al volver: {ex.Message}", "OK");
        }
    }
}