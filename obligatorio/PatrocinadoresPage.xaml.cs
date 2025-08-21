namespace obligatorio;

public partial class PatrocinadoresPage : ContentPage
{
	public PatrocinadoresPage()
	{
		InitializeComponent();
	}

	public async void btnPatrocinadoresAtras_Clicked(object sender, EventArgs e)
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