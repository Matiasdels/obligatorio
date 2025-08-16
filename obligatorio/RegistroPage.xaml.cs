

namespace obligatorio;

public partial class RegistroPage : ContentPage
{
    string imagenBase64 = null;




    public RegistroPage()
    {
        InitializeComponent();
    }

    private async void OnTomarFotoClicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();

            if (photo != null)
            {
                using var stream = await photo.OpenReadAsync();
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                imagenBase64 = Convert.ToBase64String(memoryStream.ToArray());

                imgFotoPerfil.Source = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));
            }
        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Error", "La cámara no está soportada en este dispositivo.", "OK");
        }
        catch (PermissionException)
        {
            await DisplayAlert("Permisos", "Permiso de cámara denegado.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al tomar foto: {ex.Message}", "OK");
        }
    }

    private async void OnSeleccionarFotoClicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.PickPhotoAsync();

            if (photo != null)
            {
                using var stream = await photo.OpenReadAsync();
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                imagenBase64 = Convert.ToBase64String(memoryStream.ToArray());

                imgFotoPerfil.Source = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al seleccionar foto: {ex.Message}", "OK");
        }
    }

    private void OnRegistroClicked(object sender, EventArgs e)
    {
        string usuario = txtUsuario.Text?.Trim();
        string password = txtPassword.Text;
        string nombre = txtNombre.Text?.Trim();
        string direccion = txtDireccion.Text?.Trim();
        string telefono = txtTelefono.Text?.Trim();
        string email = txtEmail.Text?.Trim();

        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(direccion) ||
            string.IsNullOrWhiteSpace(telefono) || string.IsNullOrWhiteSpace(email))
        {
            lblResultado.Text = "Todos los campos son obligatorios.";
            return;
        }

        if (!email.Contains("@"))
        {
            lblResultado.Text = "Email inválido.";
            return;
        }

        // Aquí podrías guardar los datos a base de datos, o enviarlos a una API

        lblResultado.TextColor = Colors.Green;
        lblResultado.Text = "Usuario registrado correctamente.";
    }
}
