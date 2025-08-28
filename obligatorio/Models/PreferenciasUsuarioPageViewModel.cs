using obligatorio.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace obligatorio.Models
{
    public class PreferenciasUsuarioPageViewModel : INotifyPropertyChanged
    {
        private readonly IPreferenciasUsuarioService _preferenciasService;
        private readonly IUsuarioService _usuarioService;
        private PreferenciasUsuario _preferenciasActuales;
        private bool _isLoading;

        public PreferenciasUsuario PreferenciasActuales
        {
            get => _preferenciasActuales;
            set
            {
                _preferenciasActuales = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

       
        public ICommand GuardarPreferenciasCommand { get; }
        public ICommand ActivarTodosCommand { get; }

        public PreferenciasUsuarioPageViewModel(
            IPreferenciasUsuarioService preferenciasService,
            IUsuarioService usuarioService)
        {
            _preferenciasService = preferenciasService;
            _usuarioService = usuarioService;

            GuardarPreferenciasCommand = new Command(async () => await GuardarPreferenciasAsync());
            ActivarTodosCommand = new Command(async () => await ActivarTodosModulos());

            _preferenciasService.PreferenciasCambiadas += OnPreferenciasCambiadas;
        }

        public async Task InicializarAsync()
        {
            try
            {
                IsLoading = true;

                if (_usuarioService.EstaLogueado)
                {
                    await _preferenciasService.CargarPreferenciasAsync(_usuarioService.UsuarioActual.Id);
                    PreferenciasActuales = _preferenciasService.PreferenciasActuales;
                }
                else
                {
                    
                    await MostrarError("No hay usuario logueado");
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                
            }
            catch (Exception ex)
            {
                await MostrarError($"Error al cargar preferencias: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task GuardarPreferenciasAsync()
        {
            try
            {
                IsLoading = true;

                if (PreferenciasActuales != null)
                {
                    await _preferenciasService.GuardarPreferenciasAsync();
                    await MostrarExito("Preferencias guardadas correctamente");
                }
            }
            catch (Exception ex)
            {
                await MostrarError($"Error al guardar preferencias: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ActivarTodosModulos()
        {
            try
            {
                if (PreferenciasActuales == null) return;

                var resultado = await Application.Current.MainPage.DisplayAlert(
                    "Activar Todos",
                    "¿Deseas activar todos los módulos en tu página principal?",
                    "Sí",
                    "Cancelar");

                if (!resultado) return;

                IsLoading = true;

                PreferenciasActuales.MostrarClima = true;
                PreferenciasActuales.MostrarCotizaciones = true;
                PreferenciasActuales.MostrarNoticias = true;
                PreferenciasActuales.MostrarCine = true;
                PreferenciasActuales.MostrarPatrocinadores = true;
                PreferenciasActuales.MostrarClientes = true;

                OnPropertyChanged(nameof(PreferenciasActuales));

                await GuardarPreferenciasAsync();
            }
            catch (Exception ex)
            {
                await MostrarError($"Error al activar todos los módulos: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OnPreferenciasCambiadas(object sender, PreferenciasUsuario preferencias)
        {
            PreferenciasActuales = preferencias;
        }

        private async Task MostrarError(string mensaje)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", mensaje, "OK");
                
            }
        }

        private async Task MostrarExito(string mensaje)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", mensaje, "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
