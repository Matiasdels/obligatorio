using obligatorio.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace obligatorio.Models
{
    public class BannerUsuarioViewModel : INotifyPropertyChanged
    {
        private readonly IUsuarioService _usuarioService;

        public BannerUsuarioViewModel(IUsuarioService usuarioService)
        {
            System.Diagnostics.Debug.WriteLine("Creando BannerUsuarioViewModel");

            if (usuarioService == null)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: usuarioService es nulo en BannerUsuarioViewModel");
                throw new ArgumentNullException(nameof(usuarioService));
            }

            _usuarioService = usuarioService;

            // Suscribirse al evento solo si el servicio no es nulo
            if (_usuarioService != null)
            {
                _usuarioService.PropertyChanged += OnUsuarioServicePropertyChanged;
            }

            // Comandos
            ToggleFlyoutCommand = new Command(OnToggleFlyout);
            IrAPerfilCommand = new Command(OnIrAPerfil);
            IrAPreferenciasCommand = new Command(OnIrAPreferencias);
            CerrarSesionCommand = new Command(OnCerrarSesion);

            System.Diagnostics.Debug.WriteLine("BannerUsuarioViewModel creado correctamente");
        }

        // Propiedades que se actualizan automáticamente
        public string NombreUsuario => _usuarioService?.UsuarioActual?.Nombre ?? "Invitado";
        public string RolUsuario => _usuarioService?.UsuarioActual?.Rol ?? "Sin rol";
        public bool EstaLogueado => _usuarioService?.EstaLogueado ?? false;
        public string InicalesUsuario => GenerarIniciales(NombreUsuario);

        // Comandos
        public ICommand ToggleFlyoutCommand { get; }
        public ICommand IrAPerfilCommand { get; }
        public ICommand IrAPreferenciasCommand { get; }
        public ICommand CerrarSesionCommand { get; }

        // Eventos
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnUsuarioServicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Actualizar las propiedades cuando cambien en el servicio
            OnPropertyChanged(nameof(NombreUsuario));
            OnPropertyChanged(nameof(RolUsuario));
            OnPropertyChanged(nameof(EstaLogueado));
            OnPropertyChanged(nameof(InicalesUsuario));
        }

        private void OnToggleFlyout()
        {
            try
            {
                if (Shell.Current != null)
                {
                    Shell.Current.FlyoutIsPresented = !Shell.Current.FlyoutIsPresented;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en OnToggleFlyout: {ex.Message}");
            }
        }

        private async void OnIrAPerfil()
        {
            try
            {
                if (Shell.Current != null)
                {
                    Shell.Current.FlyoutIsPresented = false;
                    await Shell.Current.GoToAsync("//Perfil");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en OnIrAPerfil: {ex.Message}");
            }
        }

        private async void OnIrAPreferencias()
        {
            try
            {
                if (Shell.Current != null)
                {
                    Shell.Current.FlyoutIsPresented = false;
                    await Shell.Current.GoToAsync("//PreferenciasPage");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en OnIrAPreferencias: {ex.Message}");
            }
        }

        private void OnCerrarSesion()
        {
            try
            {
                _usuarioService?.CerrarSesion();
                if (Shell.Current != null)
                {
                    Shell.Current.FlyoutIsPresented = false;
                    Shell.Current.GoToAsync("//LoginPage");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en OnCerrarSesion: {ex.Message}");
            }
        }

        private string GenerarIniciales(string nombre)
        {
            if (string.IsNullOrEmpty(nombre)) return "?";

            var palabras = nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (palabras.Length >= 2)
                return $"{palabras[0][0]}{palabras[1][0]}".ToUpper();
            else if (palabras.Length == 1)
                return palabras[0].Substring(0, Math.Min(2, palabras[0].Length)).ToUpper();

            return "?";
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
