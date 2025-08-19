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
    public class PerfilPageViewModel : INotifyPropertyChanged
    {
        private Usuario _usuario;
        private ImageSource _fotoPerfilSource;
        private bool _mostrarIniciales = true;

        public PerfilPageViewModel()
        {
            CambiarFotoCommand = new Command(async () => await CambiarFoto());
            CargarDatosUsuario();
        }

        // Propiedades
        public Usuario Usuario
        {
            get => _usuario;
            set
            {
                _usuario = value;
                OnPropertyChanged();
                ActualizarFotoPerfil();
                OnPropertyChanged(nameof(InicalesUsuario));
            }
        }

        public ImageSource FotoPerfilSource
        {
            get => _fotoPerfilSource;
            set
            {
                _fotoPerfilSource = value;
                OnPropertyChanged();
            }
        }

        public bool MostrarIniciales
        {
            get => _mostrarIniciales;
            set
            {
                _mostrarIniciales = value;
                OnPropertyChanged();
            }
        }

        public string InicalesUsuario
        {
            get
            {
                if (Usuario?.Nombre == null) return "?";

                var nombres = Usuario.Nombre.Split(' ');
                if (nombres.Length >= 2)
                {
                    return $"{nombres[0][0]}{nombres[1][0]}".ToUpper();
                }
                return Usuario.Nombre.Length > 0 ? Usuario.Nombre[0].ToString().ToUpper() : "?";
            }
        }

        // Comandos
        public ICommand CambiarFotoCommand { get; }

        // Métodos
        private async void CargarDatosUsuario()
        {
            try
            {
                // Aquí cargas los datos del usuario desde tu servicio/base de datos
                // Por ejemplo:
                // Usuario = await _usuarioService.ObtenerUsuarioActualAsync();

                // Datos de ejemplo (reemplaza con tu lógica):
                Usuario = new Usuario
                {
                    Id = 1,
                    Nombre = "Juan Pérez",
                    Email = "juan.perez@email.com",
                    Rol = "Administrador",
                    Telefono = "+598 99 123 456",
                    Direccion = "Av. 18 de Julio 1234, Montevideo, Uruguay",
                    FotoBase64 = null // Se cargará desde la base de datos
                };
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"No se pudieron cargar los datos del usuario: {ex.Message}", "OK");
            }
        }

        private void ActualizarFotoPerfil()
        {
            if (Usuario?.FotoBase64 != null)
            {
                try
                {
                    var imageBytes = Convert.FromBase64String(Usuario.FotoBase64);
                    FotoPerfilSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    MostrarIniciales = false;
                }
                catch
                {
                    // Si hay error al cargar la imagen, mostrar iniciales
                    FotoPerfilSource = null;
                    MostrarIniciales = true;
                }
            }
            else
            {
                FotoPerfilSource = null;
                MostrarIniciales = true;
            }
        }

        private async Task CambiarFoto()
        {
            try
            {
                var result = await Application.Current.MainPage.DisplayActionSheet(
                    "Cambiar foto de perfil",
                    "Cancelar",
                    null,
                    "Cámara",
                    "Galería");

                FileResult photo = null;

                switch (result)
                {
                    case "Cámara":
                        photo = await MediaPicker.Default.CapturePhotoAsync();
                        break;
                    case "Galería":
                        photo = await MediaPicker.Default.PickPhotoAsync();
                        break;
                    default:
                        return;
                }

                if (photo != null)
                {
                    await ProcesarNuevaFoto(photo);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"No se pudo cambiar la foto: {ex.Message}", "OK");
            }
        }

        private async Task ProcesarNuevaFoto(FileResult photo)
        {
            try
            {
                using var stream = await photo.OpenReadAsync();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                var imageBytes = memoryStream.ToArray();
                var base64String = Convert.ToBase64String(imageBytes);

                // Actualizar el usuario con la nueva foto
                Usuario.FotoBase64 = base64String;

                // Guardar en la base de datos
                await GuardarFotoEnBaseDatos(base64String);

                // Actualizar la interfaz
                ActualizarFotoPerfil();

                await Application.Current.MainPage.DisplayAlert("Éxito",
                    "Foto de perfil actualizada correctamente", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"No se pudo procesar la imagen: {ex.Message}", "OK");
            }
        }

        private async Task GuardarFotoEnBaseDatos(string fotoBase64)
        {
            try
            {
                // Aquí implementas la lógica para guardar en tu base de datos
                // Por ejemplo:
                // await _usuarioService.ActualizarFotoPerfilAsync(Usuario.Id, fotoBase64);

                // Simulación de guardado exitoso
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar en la base de datos: {ex.Message}");
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

