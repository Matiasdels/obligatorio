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
}