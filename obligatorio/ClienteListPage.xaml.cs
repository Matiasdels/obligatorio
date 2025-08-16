using obligatorio.Data;
using obligatorio.Models;
using System.Collections.ObjectModel;

namespace obligatorio;

public partial class ClienteListPage : ContentPage
{
    private DataBaseService _dbService;
    private ObservableCollection<Cliente> _clientes;

    

    public ClienteListPage()
    {
        InitializeComponent();
        _dbService = App.Database;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var clientes = await _dbService.GetClientesAsync();
        _clientes = new ObservableCollection<Cliente>(clientes);
        ClientesCollection.ItemsSource = _clientes;
    }

    private async void OnAddClienteClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ClienteDetailPage(_dbService));
    }

    private async void OnClienteSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0)
            return;

        var cliente = e.CurrentSelection[0] as Cliente;
        await Navigation.PushAsync(new ClienteDetailPage(_dbService, cliente));
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
