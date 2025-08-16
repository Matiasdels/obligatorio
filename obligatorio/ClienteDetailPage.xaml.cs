using obligatorio.Data;
using obligatorio.Models;

namespace obligatorio;

public partial class ClienteDetailPage : ContentPage
{
    private DataBaseService _dbService;
    private Cliente _cliente;

    public ClienteDetailPage(DataBaseService dbService, Cliente cliente = null)
    {
        InitializeComponent();
        _dbService = dbService;
        _cliente = cliente;

        if (_cliente != null)
        {
            NombreEntry.Text = _cliente.Nombre;
            TelefonoEntry.Text = _cliente.Telefono;
            EmailEntry.Text = _cliente.Email;
            PasswordEntry.IsVisible = false; // No mostrar la contraseña en edición
            EliminarBtn.IsVisible = true;
        }
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NombreEntry.Text) || string.IsNullOrWhiteSpace(EmailEntry.Text))
        {
            await DisplayAlert("Error", "Nombre y Email son obligatorios.", "OK");
            return;
        }

        if (_cliente == null)
        {
            _cliente = new Cliente();
        }

        _cliente.Nombre = NombreEntry.Text.Trim();
        _cliente.Telefono = TelefonoEntry.Text?.Trim();
        _cliente.Email = EmailEntry.Text.Trim();

        if (PasswordEntry.IsVisible)
        {
            // Aquí deberías aplicar hash a la contraseña antes de guardar.
            _cliente.PasswordHash = PasswordEntry.Text.Trim();
        }

        await _dbService.SaveClienteAsync(_cliente);
        await Navigation.PushAsync(new ClienteListPage());
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        var respuesta = await DisplayAlert("Confirmar", "¿Eliminar cliente?", "Sí", "No");
        if (respuesta && _cliente != null)
        {
            await _dbService.DeleteClienteAsync(_cliente);
            await Navigation.PopAsync();
        }
    }
}