namespace obligatorio;

public partial class NoticiasPage : ContentPage
{
	public NoticiasPage()
	{
		InitializeComponent();
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