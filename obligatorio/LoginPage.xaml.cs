namespace obligatorio;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void BtnLogin_Clicked(object sender, EventArgs e)
    {
        string usuario = txtUsuario.Text;
        string password = txtPassword.Text;

        // Validaci�n simple (en un caso real usar�as una API o base de datos)
        if (usuario == "admin" && password == "1234")
        {
            // Ir a la p�gina principal
            await Shell.Current.GoToAsync($"//MainPage");
        }
        else
        {
            await DisplayAlert("Error", "Usuario o contrase�a incorrectos", "OK");
        }
    }
}