using obligatorio.Models;

namespace obligatorio;

public partial class PreferenciasPage : ContentPage
{
    private PreferenciasPageViewModel _viewModel;

    public PreferenciasPage()
    {
        InitializeComponent();
        _viewModel = new PreferenciasPageViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Opcional: Recargar preferencias cuando la p�gina aparece
        // _viewModel.RecargarPreferencias(); // Si implementas este m�todo
    }

    // Evento espec�fico para el cambio de tema
    private void OnTemaOscuroToggled(object sender, ToggledEventArgs e)
    {
        if (_viewModel != null)
        {
            // Aplicar el tema inmediatamente
            _viewModel.AplicarTema(e.Value);
        }
    }


}