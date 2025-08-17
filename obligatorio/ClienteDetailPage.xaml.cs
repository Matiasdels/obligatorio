using obligatorio.Data;
using obligatorio.Models;
using Microsoft.Maui.Controls;

namespace obligatorio;

[QueryProperty(nameof(ClienteId), "clienteId")]
public partial class ClienteDetailPage : ContentPage
{
    private DataBaseService _dbService;
    private Cliente _cliente;

    private int clienteId;
    public int ClienteId
    {
        get => clienteId;
        set
        {
            clienteId = value;
            LoadCliente(clienteId);
        }
    }

    public ClienteDetailPage()
    {
        InitializeComponent();
        _dbService = App.Database;
    }

    // Método para cargar los datos de un cliente existente
    private async void LoadCliente(int id)
    {
        if (id == 0)
        {
            _cliente = null;
            txtNombre.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            btnEliminar.IsVisible = false; // No hay cliente existente
            return;
        }

        _cliente = await _dbService.GetClienteByIdAsync(id);

        if (_cliente != null)
        {
            txtNombre.Text = _cliente.Nombre;
            txtEmail.Text = _cliente.Email;
            txtTelefono.Text = _cliente.Telefono;

            btnEliminar.IsVisible = true; // Mostrar botón de eliminar solo si existe
        }
        else
        {
            // Si no se encuentra el cliente
            txtNombre.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            btnEliminar.IsVisible = false;
        }
    }

    // Guardar cliente (nuevo o existente)
    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        if (_cliente == null)
            _cliente = new Cliente();

        _cliente.Nombre = txtNombre.Text;
        _cliente.Email = txtEmail.Text;
        _cliente.Telefono = txtTelefono.Text;

        if (ClienteId == 0)
            await _dbService.AddClienteAsync(_cliente); // Crear nuevo
        else
            await _dbService.UpdateClienteAsync(_cliente); // Actualizar existente

        // Volver atrás con Shell
        await Shell.Current.GoToAsync("//ClienteListPage");
    }

    // Eliminar cliente
    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        if (_cliente == null)
            return;

        var respuesta = await DisplayAlert("Confirmar", "¿Eliminar cliente?", "Sí", "No");
        if (!respuesta)
            return;

        await _dbService.DeleteClienteAsync(_cliente);

        // Volver atrás a la lista
        await Shell.Current.GoToAsync("//ClienteListPage");
    }

    // Botón volver (opcional si querés un botón físico en la página)
    private async void btnAtras_Clicked(object sender, EventArgs e)
    {
        try
        {
            var navStack = Shell.Current.Navigation.NavigationStack;
            if (navStack.Count > 1)
                await Shell.Current.GoToAsync("..");
            else
                await Shell.Current.GoToAsync("//ClienteListPage");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}


//if (_cliente != null)
//{
//    NombreEntry.Text = _cliente.Nombre;
//    TelefonoEntry.Text = _cliente.Telefono;
//    EmailEntry.Text = _cliente.Email;
//    PasswordEntry.IsVisible = false; // No mostrar la contraseña en edición
//    EliminarBtn.IsVisible = true;
//}