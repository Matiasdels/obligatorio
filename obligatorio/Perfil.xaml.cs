using obligatorio.Models;
using obligatorio.Services;

namespace obligatorio;

public partial class Perfil : ContentPage
{
    public Perfil()
    {
        InitializeComponent();
        BindingContext = new PerfilPageViewModel(App.UsuarioService);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}