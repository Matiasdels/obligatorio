using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace obligatorio;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();

        if (DeviceInfo.Platform != DevicePlatform.Android && DeviceInfo.Platform != DevicePlatform.iOS)
        {
            // Mostrar login manual directamente
            MostrarLoginManual();
            btnHuella.IsVisible = false;
        }
    }

    private async void btnHuella_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Verificar plataforma
            if (DeviceInfo.Platform != DevicePlatform.Android && DeviceInfo.Platform != DevicePlatform.iOS)
            {
                await DisplayAlert("No disponible", "La autenticación por huella solo está disponible en Android/iOS.", "OK");
                MostrarLoginManual();
                return;
            }

            // Verificar disponibilidad de huella
            var availability = await CrossFingerprint.Current.GetAvailabilityAsync();
            if (availability != FingerprintAvailability.Available)
            {
                await DisplayAlert("No disponible", "No se encontró lector de huellas o no está configurado.", "OK");
                MostrarLoginManual();
                return;
            }

            // Intentar autenticación
            var request = new AuthenticationRequestConfiguration("Autenticación requerida", "Use su huella para ingresar");
            var result = await CrossFingerprint.Current.AuthenticateAsync(request);

            if (result.Authenticated)
            {
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await DisplayAlert("Error", "Huella no reconocida. Ingrese usuario y contraseña.", "OK");
                MostrarLoginManual();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error inesperado", $"No se pudo autenticar: {ex.Message}", "Cerrar");
            MostrarLoginManual();
        }
    }

    private void MostrarLoginManual()
    {
        lblFallback.IsVisible = true;
        txtUsuario.IsVisible = true;
        txtPassword.IsVisible = true;
        btnLogin.IsVisible = true;
    }

    private async void BtnLogin_Clicked(object sender, EventArgs e)
    {
        if (txtUsuario.Text == "admin" && txtPassword.Text == "1234")
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await DisplayAlert("Error", "Usuario o contraseña incorrectos.", "Cerrar");
        }
    }
}
