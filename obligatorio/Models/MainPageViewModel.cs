using obligatorio.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace obligatorio.Models
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly IPreferenciasUsuarioService _preferenciasService;
        private readonly IUsuarioService _usuarioService;
        private PreferenciasUsuario _preferenciasActuales;

        public PreferenciasUsuario PreferenciasActuales
        {
            get => _preferenciasActuales;
            set
            {
                _preferenciasActuales = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel(IPreferenciasUsuarioService preferenciasService, IUsuarioService usuarioService)
        {
            _preferenciasService = preferenciasService;
            _usuarioService = usuarioService;

            _preferenciasService.PreferenciasCambiadas += OnPreferenciasCambiadas;
            _usuarioService.UsuarioCambiado += OnUsuarioCambiado;
        }

        public async Task InicializarAsync()
        {
            if (_usuarioService.EstaLogueado)
            {
                await _preferenciasService.CargarPreferenciasAsync(_usuarioService.UsuarioActual.Id);
                PreferenciasActuales = _preferenciasService.PreferenciasActuales;
            }
        }

        private void OnPreferenciasCambiadas(object sender, PreferenciasUsuario preferencias)
        {
            PreferenciasActuales = preferencias;
        }

        private async void OnUsuarioCambiado(object sender, Usuario usuario)
        {
            if (usuario != null)
            {
                await _preferenciasService.CargarPreferenciasAsync(usuario.Id);
                PreferenciasActuales = _preferenciasService.PreferenciasActuales;
            }
            else
            {
                _preferenciasService.LimpiarPreferencias();
                PreferenciasActuales = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
