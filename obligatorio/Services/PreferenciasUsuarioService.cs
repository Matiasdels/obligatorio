using obligatorio.Data;
using obligatorio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace obligatorio.Services
{
    public interface IPreferenciasUsuarioService : INotifyPropertyChanged
    {
        PreferenciasUsuario PreferenciasActuales { get; }
        Task CargarPreferenciasAsync(int usuarioId);
        Task GuardarPreferenciasAsync();
        void LimpiarPreferencias();
        event EventHandler<PreferenciasUsuario> PreferenciasCambiadas;
    }

    public class PreferenciasUsuarioService : IPreferenciasUsuarioService
    {
        private readonly DataBaseService _dataBaseService;
        private PreferenciasUsuario _preferenciasActuales;

        public PreferenciasUsuario PreferenciasActuales
        {
            get => _preferenciasActuales;
            private set
            {
                if (_preferenciasActuales != value)
                {
                    // Desuscribirse del anterior
                    if (_preferenciasActuales != null)
                        _preferenciasActuales.PropertyChanged -= OnPreferenciasPropertyChanged;

                    _preferenciasActuales = value;

                    // Suscribirse al nuevo
                    if (_preferenciasActuales != null)
                        _preferenciasActuales.PropertyChanged += OnPreferenciasPropertyChanged;

                    OnPropertyChanged();
                    PreferenciasCambiadas?.Invoke(this, value);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<PreferenciasUsuario> PreferenciasCambiadas;

        public PreferenciasUsuarioService()
        {
            _dataBaseService = App.Database;
        }

        public async Task CargarPreferenciasAsync(int usuarioId)
        {
            try
            {
                PreferenciasActuales = await _dataBaseService.GetPreferenciasUsuarioAsync(usuarioId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar preferencias: {ex.Message}");
                // Crear preferencias por defecto en caso de error
                PreferenciasActuales = new PreferenciasUsuario
                {
                    UsuarioId = usuarioId,
                    MostrarClima = true,
                    MostrarCotizaciones = true,
                    MostrarNoticias = true,
                    MostrarCine = true,
                    MostrarPatrocinadores = true,
                    MostrarClientes = true
                };
            }
        }

        public async Task GuardarPreferenciasAsync()
        {
            try
            {
                if (PreferenciasActuales != null)
                {
                    await _dataBaseService.SavePreferenciasUsuarioAsync(PreferenciasActuales);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al guardar preferencias: {ex.Message}");
                throw;
            }
        }

        public void LimpiarPreferencias()
        {
            PreferenciasActuales = null;
        }

        private void OnPreferenciasPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Propagar el evento cuando cambien las preferencias individuales
            OnPropertyChanged($"PreferenciasActuales.{e.PropertyName}");

            // Auto-guardar cuando cambie alguna preferencia (opcional)
            _ = Task.Run(async () => await GuardarPreferenciasAsync());
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
