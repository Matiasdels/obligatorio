using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using obligatorio.Data;
using obligatorio.Models;

namespace obligatorio;

public partial class PatrocinadoresPage : ContentPage
{

    private readonly DataBaseService _db;
    private string _logoRuta;
    private double? _lat;
    private double? _lng;

    public PatrocinadoresPage()
	{
		InitializeComponent();
		_db = App.Database;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var location = await ObtenerUbicacionAsync();
        if (location != null)
        {
            mapaPatrocinadores.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Location(location.Latitude, location.Longitude),
                    Distance.FromKilometers(5)
                )
            );
        }
        await CargarPatrocinadoresAsync();
    }

    private async Task CargarPatrocinadoresAsync()
    {
        var patrocinadores = await _db.GetPatrocinadoresAsync();
        mapaPatrocinadores.Pins.Clear();

        foreach (var p in patrocinadores)
        {
            if (p.Latitud.HasValue && p.Longitud.HasValue)
            {
                mapaPatrocinadores.Pins.Add(new Pin
                {
                    Label = p.Nombre,
                    Location = new Location(p.Lat.Value, p.Lng.Value),
                    Address = p.Direccion
                });
            }
        }
    }

    private async void OnSeleccionarLogoClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona el logo",
                FileTypes = FilePickerFileType.Images
            });

            if (result == null) return;

            // Copiar a carpeta de la app y guardar ruta
            var destFolder = Path.Combine(FileSystem.AppDataDirectory, "logos");
            Directory.CreateDirectory(destFolder);

            var destPath = Path.Combine(destFolder, $"{Guid.NewGuid()}{Path.GetExtension(result.FileName)}");
            using (var src = await result.OpenReadAsync())
            using (var dst = File.Create(destPath))
            {
                await src.CopyToAsync(dst);
            }

            _logoRuta = destPath;
            LogoImage.Source = ImageSource.FromFile(_logoRuta);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo cargar el logo: {ex.Message}", "OK");
        }
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        var nombre = NombreEntry.Text?.Trim();
        var direccion = DireccionEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            await DisplayAlert("Validación", "El nombre es obligatorio.", "OK");
            return;
        }

        // Si no tenemos coords pero sí dirección, podemos intentar geocodificar (opcional)
        if (!_lat.HasValue || !_lng.HasValue)
        {
            if (!string.IsNullOrWhiteSpace(direccion))
            {
                try
                {
                    var locations = await Geocoding.GetLocationsAsync(direccion);
                    var loc = locations?.FirstOrDefault();
                    if (loc != null)
                    {
                        _lat = loc.Latitude;
                        _lng = loc.Longitude;
                    }
                }
                catch
                {
                    // Si geocoding falla, continuamos sin coords (lo arreglaremos con el mapa)
                }
            }
        }

        var p = new Patrocinador
        {
            Nombre = nombre,
            Direccion = direccion,
            LogoRuta = _logoRuta,
            Latitud = (double)_lat,
            Longitud = (double)_lng
        };

        await _db.SavePatrocinadorAsync(p);
        await DisplayAlert("OK", "Patrocinador guardado.", "Cerrar");

        // Limpiar y recargar
        NombreEntry.Text = string.Empty;
        DireccionEntry.Text = string.Empty;
        LogoImage.Source = null;
        _logoRuta = null;
        _lat = _lng = null;
        CoordsLabel.Text = "Sin coordenadas";

        await CargarPatrocinadoresAsync();
    }
    public async void btnPatrocinadoresAtras_Clicked(object sender, EventArgs e)
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
    private async Task<Location> ObtenerUbicacionAsync()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status == PermissionStatus.Granted)
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(10)
                    });
                }
                return location;
            }
            else
            {
                await DisplayAlert("Permiso denegado", "Se requiere acceso a la ubicación.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo obtener la ubicación: {ex.Message}", "OK");
        }
        return null;
    }
}