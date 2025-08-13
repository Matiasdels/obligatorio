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
        }

        public async Task InitAsync()
        {
            await _database.CreateTableAsync<Sucursal>();
        }

        public Task<List<Sucursal>> GetSucursalesAsync()
        {
            return _database.Table<Sucursal>().ToListAsync();
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
    }
}
