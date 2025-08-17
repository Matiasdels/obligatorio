using obligatorio.Data;
using obligatorio.Models;
using System.Collections.ObjectModel;

namespace obligatorio;

public partial class ClienteListPage : ContentPage
{
    private ObservableCollection<Cliente> _clientes;
    private DataBaseService _dbService;

    public ClienteListPage()
    {
        InitializeComponent();
        _dbService = App.Database;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadClientesAsync();
    }

    private async Task LoadClientesAsync()
    {
        var clientes = await _dbService.GetClientesAsync();
        _clientes = new ObservableCollection<Cliente>(clientes);
        ClientesCollection.ItemsSource = _clientes;
    }

    private async void OnAddClienteClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ClienteDetailPage");
    }

    private async void OnClienteSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;
        var cliente = e.CurrentSelection[0] as Cliente;
        await Shell.Current.GoToAsync($"ClienteDetailPage?clienteId={cliente.Id}");
        ((CollectionView)sender).SelectedItem = null;
    }

    private async void btnAtras_Clicked(object sender, EventArgs e)
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
}
