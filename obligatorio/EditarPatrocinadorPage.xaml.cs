using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using obligatorio.Data;
using obligatorio.Models;

namespace obligatorio;

[QueryProperty(nameof(Patrocinador), "Patrocinador")]
public partial class EditarPatrocinadorPage : ContentPage
{
    private readonly DataBaseService _databaseService;
    private Patrocinador _patrocinadorOriginal;
    private double _latitudSeleccionada;
    private double _longitudSeleccionada;
    private bool _ubicacionCambiada = false;
    private Pin _pinActual;

    public Patrocinador Patrocinador
    {
        get => _patrocinadorOriginal;
        set
        {
            _patrocinadorOriginal = value;
            OnPropertyChanged();
            if (_patrocinadorOriginal != null)
            {
                CargarDatosPatrocinador();
                InicializarMapa();
            }
        }
    }

    public EditarPatrocinadorPage()
    {
        InitializeComponent();
        _databaseService = App.Database;
    }

    private void CargarDatosPatrocinador()
    {
        if (_patrocinadorOriginal == null) return;

        NombreEntry.Text = _patrocinadorOriginal.Nombre;
        ImagenEntry.Text = _patrocinadorOriginal.Imagen;
        DireccionEntry.Text = _patrocinadorOriginal.Direccion;

        _latitudSeleccionada = _patrocinadorOriginal.Latitud;
        _longitudSeleccionada = _patrocinadorOriginal.Longitud;

        ActualizarInfoUbicacion();
    }

    private void InicializarMapa()
    {
        if (_patrocinadorOriginal == null) return;

        var ubicacionActual = new Location(_patrocinadorOriginal.Latitud, _patrocinadorOriginal.Longitud);

        // Limpiar pins existentes
        MapaEdicion.Pins.Clear();

        // Crear y configurar el pin actual
        _pinActual = new Pin
        {
            Location = ubicacionActual,
            Label = _patrocinadorOriginal.Nombre,
            Type = PinType.Place
        };

        MapaEdicion.Pins.Add(_pinActual);

        // Centrar el mapa en la ubicaci�n actual
        MapaEdicion.MoveToRegion(MapSpan.FromCenterAndRadius(ubicacionActual, Distance.FromKilometers(2)));
    }

    private void ActualizarInfoUbicacion()
    {
        UbicacionLabel.Text = $"Lat: {_latitudSeleccionada:F6}, Lng: {_longitudSeleccionada:F6}";
        UbicacionLabel.TextColor = _ubicacionCambiada ? Colors.Blue : Colors.Black;
    }

    private async void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        try
        {
            _latitudSeleccionada = e.Location.Latitude;
            _longitudSeleccionada = e.Location.Longitude;
            _ubicacionCambiada = true;

            // Actualizar la ubicaci�n del pin existente
            if (_pinActual != null)
            {
                MapaEdicion.Pins.Remove(_pinActual);
            }

            // Crear nuevo pin en la nueva ubicaci�n
            _pinActual = new Pin
            {
                Location = e.Location,
                Label = $"{_patrocinadorOriginal.Nombre} (Nueva ubicaci�n)",
                Type = PinType.Place
            };

            MapaEdicion.Pins.Add(_pinActual);

            // Actualizar la informaci�n de ubicaci�n
            ActualizarInfoUbicacion();

            // Intentar obtener la nueva direcci�n
            await ObtenerDireccionAsync(_latitudSeleccionada, _longitudSeleccionada);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al seleccionar nueva ubicaci�n: {ex.Message}", "OK");
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
            System.Diagnostics.Debug.WriteLine($"No se pudo obtener la direcci�n: {ex.Message}");
        }
    }

    private async void OnActualizarClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(NombreEntry.Text))
            {
                await DisplayAlert("Error", "El nombre es obligatorio", "OK");
                return;
            }

            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var patrocinadorActualizado = new Patrocinador
            {
                Id = _patrocinadorOriginal.Id,
                Nombre = NombreEntry.Text.Trim(),
                Imagen = ImagenEntry.Text?.Trim() ?? string.Empty,
                Latitud = _latitudSeleccionada,
                Longitud = _longitudSeleccionada,
                Direccion = DireccionEntry.Text?.Trim() ?? string.Empty
            };

            await _databaseService.SavePatrocinadorAsync(patrocinadorActualizado);

            await DisplayAlert("�xito", "Patrocinador actualizado correctamente", "OK");

            // Navegar hacia atr�s
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al actualizar: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        try
        {
            var confirmacion = await DisplayAlert("Confirmar eliminaci�n",
                $"�Est� seguro que desea eliminar el patrocinador '{_patrocinadorOriginal.Nombre}'?",
                "Eliminar", "Cancelar");

            if (confirmacion)
            {
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;

                await _databaseService.DeletePatrocinadorAsync(_patrocinadorOriginal);

                await DisplayAlert("�xito", "Patrocinador eliminado correctamente", "OK");

                // Navegar hacia atr�s
                await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al eliminar: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        // Verificar si hay cambios
        bool hayCambios = NombreEntry.Text != _patrocinadorOriginal.Nombre ||
                          ImagenEntry.Text != _patrocinadorOriginal.Imagen ||
                          DireccionEntry.Text != _patrocinadorOriginal.Direccion ||
                          _ubicacionCambiada;

        if (hayCambios)
        {
            var confirmacion = await DisplayAlert("Confirmar",
                "�Est� seguro que desea cancelar? Se perder�n los cambios realizados.",
                "S�", "No");

            if (!confirmacion) return;
        }

        await Shell.Current.GoToAsync("..");
    }
}