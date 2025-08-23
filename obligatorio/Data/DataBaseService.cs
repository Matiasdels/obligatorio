using obligatorio.Models;
using SQLite;

namespace obligatorio.Data
{
    public class DataBaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DataBaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            InitAsync().ConfigureAwait(false);
        }

        private async Task CrearAdminPorDefecto()
        {
            // Verificar si ya existe un admin
            var adminExistente = await _database.Table<Usuario>()
                .Where(u => u.Rol == "Administrador")
                .FirstOrDefaultAsync();

            if (adminExistente == null)
            {
                var admin = new Usuario
                {
                    Nombre = "admin",
                    Email = "admin@admin.com",
                    Password = "1234", // puedes cambiar la contraseña por defecto
                    Rol = "Administrador"
                };

                await _database.InsertAsync(admin);
            }
        }

        public async Task InitAsync()
        {
            await _database.CreateTableAsync<Sucursal>();
            await _database.CreateTableAsync<Cliente>();
            await _database.CreateTableAsync<Favorito>();
            await _database.CreateTableAsync<Usuario>();
            await _database.CreateTableAsync<Patrocinador>();
            var count = await _database.Table<Patrocinador>().CountAsync();
            if (count == 0)
            {
                await InsertSampleData();
            }
            await CrearAdminPorDefecto();
        }

        private async Task InsertSampleData()
        {
            var patrocinadores = new List<Patrocinador>
            {
                new Patrocinador
                {
                    Nombre = "Restaurant El Buen Sabor",
                    Imagen = "restaurant_icon.png",
                    Latitud = -34.9011,
                    Longitud = -56.1645,
                    Direccion = "Av. 18 de Julio 1234, Montevideo"
                },
                new Patrocinador
                {
                    Nombre = "Auto Service Premium",
                    Imagen = "car_service_icon.png",
                    Latitud = -34.8941,
                    Longitud = -56.1591,
                    Direccion = "Bvar. Artigas 5678, Montevideo"
                },
                new Patrocinador
                {
                    Nombre = "Farmacia Central",
                    Imagen = "pharmacy_icon.png",
                    Latitud = -34.9067,
                    Longitud = -56.1929,
                    Direccion = "8 de Octubre 2345, Montevideo"
                }
            };
            foreach (var patrocinador in patrocinadores)
            {
                await _database.InsertAsync(patrocinador);
            }
        }

        public Task<List<Sucursal>> GetSucursalesAsync()
        {
            return _database.Table<Sucursal>().ToListAsync();
        }

        public Task<Sucursal> GetSucursalAsync(int id)
        {
            return _database.Table<Sucursal>()
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> SaveSucursalAsync(Sucursal sucursal)
        {
            if (sucursal.Id != 0)
                return _database.UpdateAsync(sucursal);
            else
                return _database.InsertAsync(sucursal);
        }

        public Task<int> DeleteSucursalAsync(Sucursal sucursal)
        {
            return _database.DeleteAsync(sucursal);
        }

        // ---------- CRUD Cliente ----------

        public Task<List<Cliente>> GetClientesAsync()
        {
            return _database.Table<Cliente>().ToListAsync();
        }
        public Task<Usuario> GetUsuarioByEmailOrNombreAsync(string valor)
        {
            return _database.Table<Usuario>()
                .Where(u => u.Email == valor || u.Nombre == valor)
                .FirstOrDefaultAsync();
        }

        public Task<Cliente> GetClienteAsync(int id)
        {
            return _database.Table<Cliente>()
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<Cliente> GetClienteByEmailAsync(string email)
        {
            return _database.Table<Cliente>()
                .Where(c => c.Email == email)
                .FirstOrDefaultAsync();
        }

        public Task<int> SaveClienteAsync(Cliente cliente)
        {
            if (cliente.Id != 0)
                return _database.UpdateAsync(cliente);
            else
                return _database.InsertAsync(cliente);
        }

        public Task<int> DeleteClienteAsync(Cliente cliente)
        {
            return _database.DeleteAsync(cliente);
        }

        // ---------- CRUD Favorito ----------

        public Task<List<Favorito>> GetFavoritosPorClienteAsync(int clienteId)
        {
            return _database.Table<Favorito>()
                .Where(f => f.ClienteId == clienteId)
                .ToListAsync();
        }

        public Task<Favorito> GetFavoritoAsync(int id)
        {
            return _database.Table<Favorito>()
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> SaveFavoritoAsync(Favorito favorito)
        {
            if (favorito.Id != 0)
                return _database.UpdateAsync(favorito);
            else
                return _database.InsertAsync(favorito);
        }

        public Task<int> DeleteFavoritoAsync(Favorito favorito)
        {
            return _database.DeleteAsync(favorito);
        }

        // ---------- CRUD Usuario ----------
        public Task<List<Usuario>> GetUsuariosAsync() =>
            _database.Table<Usuario>().ToListAsync();

        public Task<Usuario> GetUsuarioAsync(int id) =>
            _database.Table<Usuario>().Where(u => u.Id == id).FirstOrDefaultAsync();

        public async Task<Usuario> GetUsuarioByEmailAsync(string email)
        {
            return await _database.Table<Usuario>()
                                  .Where(u => u.Email == email)
                                  .FirstOrDefaultAsync();
        }


        public Task<int> SaveUsuarioAsync(Usuario usuario)
        {
            if (usuario.Id != 0)
                return _database.UpdateAsync(usuario);
            else
                return _database.InsertAsync(usuario);
        }

        public Task<int> DeleteUsuarioAsync(Usuario usuario) =>
            _database.DeleteAsync(usuario);

        // Alias para compatibilidad con ClienteDetailPage
        public Task<Cliente> GetClienteByIdAsync(int id) => GetClienteAsync(id);

        public Task<int> AddClienteAsync(Cliente cliente) => SaveClienteAsync(cliente);

        public Task<int> UpdateClienteAsync(Cliente cliente) => SaveClienteAsync(cliente);

        public async Task<Usuario> GetUsuarioConHuellaAsync()
        {
            return await _database.Table<Usuario>()
                           .Where(u => u.HuellaRegistrada == true)
                           .FirstOrDefaultAsync();
        }



        public async Task<List<Patrocinador>> GetPatrocinadoresAsync()
        {
            return await _database.Table<Patrocinador>().OrderBy(p => p.Nombre).ToListAsync();
        }

        public async Task<Patrocinador> GetPatrocinadorByIdAsync(int id)
        {
            return await _database.Table<Patrocinador>().Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SavePatrocinadorAsync(Patrocinador patrocinador)
        {
            if (patrocinador.Id != 0)
                return await _database.UpdateAsync(patrocinador);
            else
                return await _database.InsertAsync(patrocinador);
        }

        public async Task<int> DeletePatrocinadorAsync(Patrocinador patrocinador)
        {
            return await _database.DeleteAsync(patrocinador);
        }

        public async Task<int> DeletePatrocinadorAsync(int id)
        {
            return await _database.DeleteAsync<Patrocinador>(id);
        }

    }
}
