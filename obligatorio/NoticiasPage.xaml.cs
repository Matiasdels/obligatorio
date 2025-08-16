using obligatorio.Models;
using obligatorio.Services;

namespace obligatorio;

public partial class NoticiasPage : ContentPage
{
    private readonly NoticiasViewModel _viewModel;

    public NoticiasPage()
    {
        InitializeComponent();

        _viewModel = new NoticiasViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarNoticiasAsync();
    }

    public async void btnNoticiasAtras_Clicked(object sender, EventArgs e)
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