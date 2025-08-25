using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using obligatorio.Data;
using obligatorio.Models;

namespace obligatorio;

public partial class CrearPatrocinadorPage : ContentPage
{
    private readonly DataBaseService _databaseService;
    private double _latitudSeleccionada;
    private double _longitudSeleccionada;
    private bool _ubicacionSeleccionada = false;
    private Pin _pinSeleccionado;

    public CrearPatrocinadorPage()
    {
        InitializeComponent();
        _databaseService = App.Database;
        InicializarMapa();
    }

    private void InicializarMapa()
    {
        // Centrar el mapa en una ubicación por defecto (Montevideo, Uruguay)
        var ubicacionInicial = new Location(-34.9011, -56.1645);
        MapaSeleccion.MoveToRegion(MapSpan.FromCenterAndRadius(ubicacionInicial, Distance.FromKilometers(10)));
    }

    private async void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        try
        {
            _latitudSeleccionada = e.Location.Latitude;
            _longitudSeleccionada = e.Location.Longitude;
            _ubicacionSeleccionada = true;

            // Remover el pin anterior si existe
            if (_pinSeleccionado != null)
            {
                MapaSeleccion.Pins.Remove(_pinSeleccionado);
            }

            // Crear y agregar un nuevo pin
            _pinSeleccionado = new Pin
            {
                Location = e.Location,
                Label = "Ubicación seleccionada",
                Type = PinType.Place
            };

            MapaSeleccion.Pins.Add(_pinSeleccionado);

            // Actualizar la información de ubicación
            UbicacionLabel.Text = $"Lat: {_latitudSeleccionada:F6}, Lng: {_longitudSeleccionada:F6}";
            UbicacionLabel.TextColor = Colors.Black;

            // Intentar obtener la dirección (geocodificación inversa)
            await ObtenerDireccionAsync(_latitudSeleccionada, _longitudSeleccionada);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al seleccionar ubicación: {ex.Message}", "OK");
        }
    }

    private async Task ObtenerDireccionAsync(double latitud, double longitud)
    {
        try
        {
            var placemarks = await Geocoding.Default.GetPlacemarksAsync(latitud, longitud);
            var placemark = placemarks?.FirstOrDefault();

            if (placemark != null)
            {
                var direccion = $"{placemark.Thoroughfare} {placemark.SubThoroughfare}, {placemark.Locality}, {placemark.CountryName}";
                DireccionEntry.Text = direccion.Replace("  ", " ").Trim().TrimStart(',').Trim();
            }
        }
        catch (Exception ex)
        {
            // Si no se puede obtener la dirección, no es un error crítico
            System.Diagnostics.Debug.WriteLine($"No se pudo obtener la dirección: {ex.Message}");
        }
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        try
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(NombreEntry.Text))
            {
                await DisplayAlert("Error", "El nombre es obligatorio", "OK");
                return;
            }

            if (!_ubicacionSeleccionada)
            {
                await DisplayAlert("Error", "Debe seleccionar una ubicación en el mapa", "OK");
                return;
            }

            // Mostrar loading
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            // Crear el nuevo patrocinador
            var nuevoPatrocinador = new Patrocinador
            {
                Nombre = NombreEntry.Text.Trim(),
                Imagen = ImagenEntry.Text?.Trim() ?? string.Empty,
                Latitud = _latitudSeleccionada,
                Longitud = _longitudSeleccionada,
                Direccion = DireccionEntry.Text?.Trim() ?? string.Empty
            };

            // Guardar en la base de datos
            await _databaseService.SavePatrocinadorAsync(nuevoPatrocinador);

            await DisplayAlert("Éxito", "Patrocinador creado correctamente", "OK");

            // Volver a la página anterior usando Shell
            await Shell.Current.GoToAsync("GestionPatrocinadoresPage");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al guardar: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        var confirmacion = await DisplayAlert("Confirmar",
            "¿Está seguro que desea cancelar? Se perderán los datos ingresados.",
            "Sí", "No");

        if (confirmacion)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}