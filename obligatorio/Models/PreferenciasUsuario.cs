using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace obligatorio.Models
{
    
    [Table("PreferenciasUsuario")]
    public class PreferenciasUsuario : INotifyPropertyChanged
    {
        private int _id;
        private int _usuarioId;
        private bool _mostrarClima = true;
        private bool _mostrarCotizaciones = true;
        private bool _mostrarNoticias = true;
        private bool _mostrarCine = true;
        private bool _mostrarPatrocinadores = true;
        private bool _mostrarClientes = true;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public int UsuarioId
        {
            get => _usuarioId;
            set
            {
                _usuarioId = value;
                OnPropertyChanged();
            }
        }

        public bool MostrarClima
        {
            get => _mostrarClima;
            set
            {
                _mostrarClima = value;
                OnPropertyChanged();
            }
        }

        public bool MostrarCotizaciones
        {
            get => _mostrarCotizaciones;
            set
            {
                _mostrarCotizaciones = value;
                OnPropertyChanged();
            }
        }

        public bool MostrarNoticias
        {
            get => _mostrarNoticias;
            set
            {
                _mostrarNoticias = value;
                OnPropertyChanged();
            }
        }

        public bool MostrarCine
        {
            get => _mostrarCine;
            set
            {
                _mostrarCine = value;
                OnPropertyChanged();
            }
        }

        public bool MostrarPatrocinadores
        {
            get => _mostrarPatrocinadores;
            set
            {
                _mostrarPatrocinadores = value;
                OnPropertyChanged();
            }
        }

        public bool MostrarClientes
        {
            get => _mostrarClientes;
            set
            {
                _mostrarClientes = value;
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
