namespace obligatorio;

public partial class CotizacionesPage : ContentPage
{
	public CotizacionesPage()
	{
		InitializeComponent();
	}

    private async void btnCotizaciones_Clicked(object sender, EventArgs e)
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