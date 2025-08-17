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
    public interface IUsuarioService : INotifyPropertyChanged
    {
        Usuario UsuarioActual { get; }
        bool EstaLogueado { get; }

        void SetUsuarioLogueado(Usuario usuario);
        void CerrarSesion();

        event EventHandler<Usuario> UsuarioCambiado;
    }
    
    public class UsuarioService : IUsuarioService
    {
        private Usuario _usuarioActual;
        
        public Usuario UsuarioActual 
        { 
            get => _usuarioActual; 
            private set 
            { 
                if (_usuarioActual != value)
                {
                    _usuarioActual = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(EstaLogueado));
                    UsuarioCambiado?.Invoke(this, value);
                }
            }
        }

        public bool EstaLogueado => UsuarioActual != null;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<Usuario> UsuarioCambiado;

        public void SetUsuarioLogueado(Usuario usuario)
        {
            UsuarioActual = usuario;
        }

        public void CerrarSesion()
        {
            UsuarioActual = null;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
