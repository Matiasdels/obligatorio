using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using obligatorio.Models;

namespace obligatorio.Data
{
    public class DataBaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DataBaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            InitAsync();
        }

        public async Task InitAsync()
        {
            await _database.CreateTableAsync<Sucursal>();
            await _database.CreateTableAsync<Cliente>();
            await _database.CreateTableAsync<Favorito>();
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
    }
}
