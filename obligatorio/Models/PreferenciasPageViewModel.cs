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
    public class PreferenciasPageViewModel : INotifyPropertyChanged
    {
        private Preferencias _preferencias;
        private bool _isLoading;

        public PreferenciasPageViewModel()
        {
            GuardarPreferenciasCommand = new Command(async () => await GuardarPreferencias());
            RestablecerPreferenciasCommand = new Command(async () => await RestablecerPreferencias());

            CargarPreferencias();
        }

        // Propiedades
        public Preferencias Preferencias
        {
            get => _preferencias;
            set
            {
                _preferencias = value;
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

        // Comandos
        public ICommand GuardarPreferenciasCommand { get; }
        public ICommand RestablecerPreferenciasCommand { get; }

        // Métodos
        private async void CargarPreferencias()
        {
            try
            {
                IsLoading = true;

                // Cargar preferencias desde almacenamiento local
                Preferencias = new Preferencias
                {
                    Notificaciones = await GetPreferenceAsync("Notificaciones", true),
                    TemaOscuro = await GetPreferenceAsync("TemaOscuro", false),
                    MusicaSegundoPlano = await GetPreferenceAsync("MusicaSegundoPlano", true),
                    AltaCalidadAudio = await GetPreferenceAsync("AltaCalidadAudio", false),
                    DescargaAutomatica = await GetPreferenceAsync("DescargaAutomatica", true)
                };

                // Aplicar tema actual
                AplicarTema(Preferencias.TemaOscuro);
            }
            catch (Exception ex)
            {
                await MostrarError($"Error al cargar preferencias: {ex.Message}");

                // Usar valores por defecto en caso de error
                Preferencias = new Preferencias();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GuardarPreferencias()
        {
            try
            {
                IsLoading = true;

                // Guardar cada preferencia individualmente
                await SetPreferenceAsync("Notificaciones", Preferencias.Notificaciones);
                await SetPreferenceAsync("TemaOscuro", Preferencias.TemaOscuro);
                await SetPreferenceAsync("MusicaSegundoPlano", Preferencias.MusicaSegundoPlano);
                await SetPreferenceAsync("AltaCalidadAudio", Preferencias.AltaCalidadAudio);
                await SetPreferenceAsync("DescargaAutomatica", Preferencias.DescargaAutomatica);

                // Aplicar cambios inmediatos
                await AplicarCambios();

                await MostrarExito("Preferencias guardadas correctamente");
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

        private async Task RestablecerPreferencias()
        {
            try
            {
                var resultado = await Application.Current.MainPage.DisplayAlert(
                    "Restablecer Preferencias",
                    "¿Estás seguro de que deseas restablecer todas las preferencias a sus valores por defecto?",
                    "Sí",
                    "Cancelar");

                if (!resultado) return;

                IsLoading = true;

                // Valores por defecto
                Preferencias.Notificaciones = true;
                Preferencias.TemaOscuro = false;
                Preferencias.MusicaSegundoPlano = true;
                Preferencias.AltaCalidadAudio = false;
                Preferencias.DescargaAutomatica = true;

                // Actualizar la UI
                OnPropertyChanged(nameof(Preferencias));

                // Guardar los valores por defecto
                await GuardarPreferencias();

                await MostrarExito("Preferencias restablecidas correctamente");
            }
            catch (Exception ex)
            {
                await MostrarError($"Error al restablecer preferencias: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AplicarCambios()
        {
            try
            {
                // Aplicar tema
                AplicarTema(Preferencias.TemaOscuro);

                // Configurar notificaciones
                if (Preferencias.Notificaciones)
                {
                    await SolicitarPermisosNotificaciones();
                }
                else
                {
                    await DeshabilitarNotificaciones();
                }

                // Configurar música en segundo plano
                ConfigurarMusicaSegundoPlano(Preferencias.MusicaSegundoPlano);

                // Configurar calidad de audio
                ConfigurarCalidadAudio(Preferencias.AltaCalidadAudio);

                // Configurar descargas automáticas
                ConfigurarDescargasAutomaticas(Preferencias.DescargaAutomatica);
            }
            catch (Exception ex)
            {
                await MostrarError($"Error al aplicar cambios: {ex.Message}");
            }
        }

        public void AplicarTema(bool temaOscuro)
        {
            try
            {
                if (Application.Current != null)
                {
                    Application.Current.UserAppTheme = temaOscuro ? AppTheme.Dark : AppTheme.Light;
                }
            }
            catch (Exception ex)
            {
                // Log del error pero no mostrar al usuario ya que es cosmético
                System.Diagnostics.Debug.WriteLine($"Error al aplicar tema: {ex.Message}");
            }
        }

        private async Task SolicitarPermisosNotificaciones()
        {
            try
            {
                // Aquí implementarías la lógica para solicitar permisos de notificaciones
                // Depende de la plataforma (Android/iOS)

#if ANDROID
                // Lógica específica para Android
#elif IOS
                // Lógica específica para iOS
#endif

                await Task.Delay(100); // Simulación
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al configurar notificaciones: {ex.Message}");
            }
        }

        private async Task DeshabilitarNotificaciones()
        {
            try
            {
                // Lógica para deshabilitar notificaciones
                await Task.Delay(100); // Simulación
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al deshabilitar notificaciones: {ex.Message}");
            }
        }

        private void ConfigurarMusicaSegundoPlano(bool habilitar)
        {
            try
            {
                // Aquí configurarías el reproductor de audio para funcionar en segundo plano
                // Esto dependería de tu implementación de audio
                System.Diagnostics.Debug.WriteLine($"Música en segundo plano: {habilitar}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al configurar música en segundo plano: {ex.Message}");
            }
        }

        private void ConfigurarCalidadAudio(bool altaCalidad)
        {
            try
            {
                // Configurar la calidad del stream de audio
                System.Diagnostics.Debug.WriteLine($"Alta calidad de audio: {altaCalidad}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al configurar calidad de audio: {ex.Message}");
            }
        }

        private void ConfigurarDescargasAutomaticas(bool habilitar)
        {
            try
            {
                // Configurar descargas automáticas
                System.Diagnostics.Debug.WriteLine($"Descargas automáticas: {habilitar}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al configurar descargas automáticas: {ex.Message}");
            }
        }

        // Métodos de utilidad para preferencias
        private async Task<bool> GetPreferenceAsync(string key, bool defaultValue)
        {
            try
            {
                return await SecureStorage.GetAsync(key) == "true" ||
                       (await SecureStorage.GetAsync(key) == null && defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        private async Task SetPreferenceAsync(string key, bool value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar preferencia {key}: {ex.Message}");
            }
        }

        // Métodos de utilidad para mensajes
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

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Modelo de Preferencias
    public class Preferencias : INotifyPropertyChanged
    {
        private bool _notificaciones = true;
        private bool _temaOscuro = false;
        private bool _musicaSegundoPlano = true;
        private bool _altaCalidadAudio = false;
        private bool _descargaAutomatica = true;

        public bool Notificaciones
        {
            get => _notificaciones;
            set
            {
                _notificaciones = value;
                OnPropertyChanged();
            }
        }

        public bool TemaOscuro
        {
            get => _temaOscuro;
            set
            {
                _temaOscuro = value;
                OnPropertyChanged();
            }
        }

        public bool MusicaSegundoPlano
        {
            get => _musicaSegundoPlano;
            set
            {
                _musicaSegundoPlano = value;
                OnPropertyChanged();
            }
        }

        public bool AltaCalidadAudio
        {
            get => _altaCalidadAudio;
            set
            {
                _altaCalidadAudio = value;
                OnPropertyChanged();
            }
        }

        public bool DescargaAutomatica
        {
            get => _descargaAutomatica;
            set
            {
                _descargaAutomatica = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


