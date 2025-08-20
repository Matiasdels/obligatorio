using obligatorio.Services;

namespace obligatorio;

public partial class CotizacionesPage : ContentPage
{
    private readonly CurrencyService _currencyService = new CurrencyService();

    public CotizacionesPage()
    {
        InitializeComponent();
        CargarCotizaciones();
    }

    private async void CargarCotizaciones()
    {
        try
        {
            var cotizaciones = await _currencyService.ObtenerCotizacionesAsync();

            if (cotizaciones.Success)
            {
                // En la API gratuita, los valores son 1 USD = X moneda, así que invertimos para UYU?USD
                double usdToUyu = cotizaciones.Quotes["USDUYU"];
                double eurToUyu = usdToUyu / cotizaciones.Quotes["USDEUR"];
                double brlToUyu = usdToUyu / cotizaciones.Quotes["USDBRL"];

                lblDolar.Text = $"Dólar: {usdToUyu:F2} UYU";
                lblEuro.Text = $"Euro: {eurToUyu:F2} UYU";
                lblReal.Text = $"Real: {brlToUyu:F2} UYU";
            }
            else
            {
                await DisplayAlert("Error", "No se pudo obtener las cotizaciones", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    // Animación para cuando el cursor entra al encabezado
    private async void OnHeaderPointerEntered(object sender, PointerEventArgs e)
    {
        if (sender is Frame frame)
        {
            await frame.ScaleTo(1.05, 200, Easing.CubicOut);
        }
    }

    // Animación para cuando el cursor sale del encabezado
    private async void OnHeaderPointerExited(object sender, PointerEventArgs e)
    {
        if (sender is Frame frame)
        {
            await frame.ScaleTo(1.0, 200, Easing.CubicOut);
        }
    }

    // Animación para cuando el cursor entra a las tarjetas de cotizaciones
    private async void OnCotizacionPointerEntered(object sender, PointerEventArgs e)
    {
        if (sender is Frame frame)
        {
            await Task.WhenAll(
                frame.ScaleTo(1.08, 200, Easing.CubicOut),
                frame.TranslateTo(0, -5, 200, Easing.CubicOut)
            );
        }
    }

    // Animación para cuando el cursor sale de las tarjetas de cotizaciones
    private async void OnCotizacionPointerExited(object sender, PointerEventArgs e)
    {
        if (sender is Frame frame)
        {
            await Task.WhenAll(
                frame.ScaleTo(1.0, 200, Easing.CubicOut),
                frame.TranslateTo(0, 0, 200, Easing.CubicOut)
            );
        }
    }

    // Métodos para actualizar las cotizaciones dinámicamente
    public void ActualizarDolar(string valor, string variacion, bool esPositiva)
    {
        lblDolar.Text = valor;
        lblDolarVariacion.Text = variacion;
        lblDolarVariacion.TextColor = esPositiva ? Colors.LightGreen : Colors.LightCoral;
    }

    public void ActualizarEuro(string valor, string variacion, bool esPositiva)
    {
        lblEuro.Text = valor;
        lblEuroVariacion.Text = variacion;
        lblEuroVariacion.TextColor = esPositiva ? Colors.LightGreen : Colors.LightCoral;
    }

    public void ActualizarReal(string valor, string variacion, bool esPositiva)
    {
        lblReal.Text = valor;
        lblRealVariacion.Text = variacion;
        lblRealVariacion.TextColor = esPositiva ? Colors.LightGreen : Colors.LightCoral;
    }

    // Método para simular la actualización de todas las cotizaciones
    public async void SimularActualizacionCotizaciones()
    {
        var random = new Random();

        while (true)
        {
            // Simular valores aleatorios para las cotizaciones
            var dolarValor = 40 + (random.NextDouble() * 5); // Entre 40 y 45
            var euroValor = 44 + (random.NextDouble() * 4);  // Entre 44 y 48
            var realValor = 8 + (random.NextDouble() * 2);   // Entre 8 y 10

            var dolarVariacion = (random.NextDouble() - 0.5) * 2; // Entre -1 y 1
            var euroVariacion = (random.NextDouble() - 0.5) * 2;
            var realVariacion = (random.NextDouble() - 0.5) * 2;

            // Actualizar las cotizaciones en el UI thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ActualizarDolar($"${dolarValor:F2}", $"{(dolarVariacion >= 0 ? "+" : "")}{dolarVariacion:F2}", dolarVariacion >= 0);
                ActualizarEuro($"${euroValor:F2}", $"{(euroVariacion >= 0 ? "+" : "")}{euroVariacion:F2}", euroVariacion >= 0);
                ActualizarReal($"${realValor:F2}", $"{(realVariacion >= 0 ? "+" : "")}{realVariacion:F2}", realVariacion >= 0);
            });

            // Esperar 5 segundos antes de la siguiente actualización
            await Task.Delay(5000);
        }
    }

    // Evento que se ejecuta cuando la página aparece
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Opcional: Iniciar la simulación de actualizaciones automáticas
        // Task.Run(SimularActualizacionCotizaciones);
    }

    private async void btnAtras_Clicked(object sender, EventArgs e)
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