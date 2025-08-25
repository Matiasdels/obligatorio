using obligatorio.Models;
using obligatorio.Services;
using System.Globalization;

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


    private void OnNewsSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel.FiltrarNoticias(e.NewTextValue);
    }
    private async void OnReadMoreClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button button && button.BindingContext is Noticia noticia)
            {
                if (!string.IsNullOrEmpty(noticia.Link))
                {
                    // Abrir en navegador
                    await Launcher.OpenAsync(noticia.Link);
                }
                else
                {
                    await DisplayAlert("Error", "No hay enlace disponible para esta noticia", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo abrir el enlace: {ex.Message}", "OK");
        }
    }


    private void OnImageLoaded(object sender, EventArgs e)
    {
        if (sender is Image image)
        {
            image.IsVisible = true;
 
            if (image.Parent is Grid grid)
            {
                var fallback = grid.Children.OfType<StackLayout>().FirstOrDefault();
                if (fallback != null)
                    fallback.IsVisible = false;
            }
        }
    }

    private async void OnHeaderPointerEntered(object sender, PointerEventArgs e)
    {
        if (sender is Frame frame)
        {
            await frame.ScaleTo(1.05, 200, Easing.CubicOut);
        }
    }

    private async void OnHeaderPointerExited(object sender, PointerEventArgs e)
    {
        if (sender is Frame frame)
        {
            await frame.ScaleTo(1.0, 200, Easing.CubicOut);
        }
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