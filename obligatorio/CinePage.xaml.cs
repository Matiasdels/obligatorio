using Microsoft.Maui.Controls;
using obligatorio.Services;
using obligatorio.ViewModels;

namespace obligatorio
{
    public partial class CinePage : ContentPage
    {
        private CinePageViewModel _viewModel;

        public CinePage()
        {
            InitializeComponent();
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new CinePageViewModel();
            BindingContext = _viewModel;
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            // Usar el comando del ViewModel
            if (_viewModel.SearchCommand.CanExecute(e.NewTextValue))
            {
                _viewModel.SearchCommand.Execute(e.NewTextValue);
            }
        }
        // Animación para cuando el cursor entra al encabezado
        private async void OnHeaderPointerEntered(object sender, PointerEventArgs e)
        {
            if (sender is Frame frame)
            {
                await frame.ScaleTo(1.08, 200, Easing.CubicOut);
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
        private async void btnCineAtras_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No se pudo navegar hacia atrás", "OK");
            }
        }
    }
}