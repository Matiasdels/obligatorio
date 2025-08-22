using obligatorio.Data;
using PencilKit;
using System.Net.NetworkInformation;

namespace obligatorio.Models;

public partial class MapaPatrocinadorPage : ContentPage
{
        private readonly DataBaseService _databaseService;
    private Patrocinador _patrocinador;
    private Location _userLocation;

    public MapaPatrocinadorPage()
    {
        InitializeComponent();
        _databaseService = new DataBaseService();
    }

    public string PatrocinadorId { get; set; }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPatrocinadorData();
        await InitializeMap();
    }

    private async Task LoadPatrocinadorData()
    {
        try
        {
            if (int.TryParse(PatrocinadorId, out int id))
            {
                _patrocinador = await _databaseService.GetPatrocinadorByIdAsync(id);

                if (_patrocinador != null)
                {
                    LblNombrePatrocinador.Text = _patrocinador.Nombre;
                    LblDireccionPatrocinador.Text = _patrocinador.Direccion ?? "Ubicación";

                    // Actualizar título de la página
                    Title = _patrocinador.Nombre;
                }
            }
        }
        catch (Exception ex)
        {
            await ShowToast($"Error al cargar datos: {ex.Message}");
        }
    }

    private async Task InitializeMap()
    {
        try
        {
            if (_patrocinador == null) return;

            // Obtener ubicación del usuario
            await GetUserLocationAsync();

            // Ubicación del patrocinador
            var patrocinadorLocation = new Location(_patrocinador.Latitud, _patrocinador.Longitud);

            // Crear pin para el patrocinador
            var patrocinadorPin = new Microsoft.Graph.Education.Classes.Item.Modules.Item.Pin
            {
                Label = _patrocinador.Nombre,
                Address = _patrocinador.Direccion,
                Type = PinType.Place,
                Location = patrocinadorLocation
            };

            MapaUbicacion.Pins.Add(patrocinadorPin);

            // Si tenemos la ubicación del usuario, centrar el mapa para mostrar ambas ubicaciones
            if (_userLocation != null)
            {
                var centerLocation = CalculateCenterLocation(_userLocation, patrocinadorLocation);
                var distance = Location.CalculateDistance(_userLocation, patrocinadorLocation, DistanceUnits.Kilometers);

                // Crear región del mapa con zoom apropiado
                var region = MapSpan.FromCenterAndRadius(centerLocation, Distance.FromKilometers(Math.Max(distance * 1.5, 1.0)));
                MapaUbicacion.MoveToRegion(region);
            }
            else
            {
                // Si no hay ubicación del usuario, centrar en el patrocinador
                var region = MapSpan.FromCenterAndRadius(patrocinadorLocation, Distance.FromKilometers(2.0));
                MapaUbicacion.MoveToRegion(region);
            }

            LoadingIndicator.IsVisible = false;
        }
        catch (Exception ex)
        {
            LoadingIndicator.IsVisible = false;
            await ShowToast($"Error al cargar el mapa: {ex.Message}");
        }
    }

    private async Task GetUserLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Medium,
                Timeout = TimeSpan.FromSeconds(10)
            };

            var location = await Geolocation.GetLocationAsync(request);
            if (location != null)
            {
                _userLocation = location;
            }
        }
        catch (Exception ex)
        {
            // Si no se puede obtener la ubicación, continuar sin ella
            System.Diagnostics.Debug.WriteLine($"Error obteniendo ubicación: {ex.Message}");
        }
    }

    private Location CalculateCenterLocation(Location loc1, Location loc2)
    {
        var centerLat = (loc1.Latitude + loc2.Latitude) / 2;
        var centerLng = (loc1.Longitude + loc2.Longitude) / 2;
        return new Location(centerLat, centerLng);
    }

    private async void OnLlamarClicked(object sender, EventArgs e)
    {
        await ShowToast("Función de llamada no disponible - No hay teléfono registrado");
    }

    private async void OnDireccionesClicked(object sender, EventArgs e)
    {
        try
        {
            if (_patrocinador != null)
            {
                var location = new Location(_patrocinador.Latitud, _patrocinador.Longitud);
                var options = new MapLaunchOptions
                {
                    Name = _patrocinador.Nombre,
                    NavigationMode = NavigationMode.Driving
                };

                await Map.Default.OpenAsync(location, options);
            }
        }
        catch (Exception ex)
        {
            await ShowToast($"Error al abrir direcciones: {ex.Message}");
        }
    }

    private async Task ShowToast(string message)
    {
        ToastText.Text = message;
        ToastMessage.IsVisible = true;

        await ToastMessage.FadeTo(1, 300);
        await Task.Delay(3000);
        await ToastMessage.FadeTo(0, 300);

        ToastMessage.IsVisible = false;
    }
}
