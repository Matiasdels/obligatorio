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

        // Validación simple (en un caso real usarías una API o base de datos)
        if (usuario == "admin" && password == "1234")
        {
            // Ir a la página principal
            await Shell.Current.GoToAsync($"//MainPage");
        }
        else
        {
            await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
        }
    }
}