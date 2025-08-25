using obligatorio.Data;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace obligatorio;

public partial class LoginPage : ContentPage
{
    private DataBaseService _dbService;

    public LoginPage(DataBaseService dbService)
    {
        InitializeComponent();
        _dbService = App.Database;
        MostrarLoginManual();
        if (DeviceInfo.Platform != DevicePlatform.Android && DeviceInfo.Platform != DevicePlatform.iOS)
        {
            // Mostrar login manual directamente
            MostrarLoginManual();
            frameHuella.IsVisible = false;
            btnHuella.IsVisible = false;
            separadorO.IsVisible = false;
        }
    }

    private async void btnHuella_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Verificar plataforma
            if (DeviceInfo.Platform != DevicePlatform.Android && DeviceInfo.Platform != DevicePlatform.iOS)
            {
                await DisplayAlert("No disponible", "La autenticaci�n por huella solo est� disponible en Android/iOS.", "OK");
                MostrarLoginManual();
                return;
            }

            // Verificar disponibilidad de huella
            var availability = await CrossFingerprint.Current.GetAvailabilityAsync();
            if (availability != FingerprintAvailability.Available)
            {
                await DisplayAlert("No disponible", "No se encontr� lector de huellas o no est� configurado.", "OK");
                MostrarLoginManual();
                return;
            }

            // Intentar autenticaci�n
            var request = new AuthenticationRequestConfiguration("Autenticaci�n requerida", "Use su huella para ingresar");
            var result = await CrossFingerprint.Current.AuthenticateAsync(request);

            if (result.Authenticated)
            {
                var usuarioConHuella = await _dbService.GetUsuarioConHuellaAsync();

                if (usuarioConHuella != null)
                {
                    App.UsuarioService.SetUsuarioLogueado(usuarioConHuella);
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    await DisplayAlert("Error", "No hay un usuario registrado con huella. Inicie sesi�n manualmente.", "OK");
                    MostrarLoginManual();
                }
            }
            else
            {
                await DisplayAlert("Error", "Huella no reconocida. Ingrese usuario y contrase�a.", "OK");
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
        
        string valor = txtUsuario.Text.Trim();
        string password = txtPassword.Text;

        var usuario = await _dbService.GetUsuarioByEmailOrNombreAsync(valor);

        if (usuario != null && usuario.Password.Trim() == password.Trim())
        {
            App.UsuarioService.SetUsuarioLogueado(usuario);
            await Shell.Current.GoToAsync("//MainPage");
            
        }
        else
        {
            await DisplayAlert("Error", "Usuario o contrase�a incorrectos.", "Cerrar");
        }
    }

    private async void OnRegistroTapped(object sender, EventArgs e)
    {
        // Aseg�rate de tener una p�gina de registro creada, por ejemplo RegistroPage.xaml
        await Navigation.PushAsync(new RegistroPage(App.Database));
    }
}
