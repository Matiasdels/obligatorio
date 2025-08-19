using obligatorio.Models;

namespace obligatorio;

public partial class Perfil : ContentPage
{
	public Perfil()
	{
		InitializeComponent();
        BindingContext = new PerfilPageViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        
    }
}