namespace obligatorio;

public partial class ClimaPage : ContentPage
{
	public ClimaPage()
	{
		InitializeComponent();
	}

    private async void btnClimaAtras_Clicked(object sender, EventArgs e)
    {
        try
        {
            var navStack = Shell.Current.Navigation.NavigationStack;

            if (navStack.Count > 1)
            {
                // Hay una página anterior, podemos volver
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                // No hay página anterior, vamos directo a MainPage
                await Shell.Current.GoToAsync("//MainPage");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}