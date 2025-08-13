using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obligatorio.Models
{
    public class Cliente
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100), NotNull]
        public string Nombre { get; set; }

        [MaxLength(30)]
        public string Telefono { get; set; }

        [MaxLength(100), Unique, NotNull]
        public string Email { get; set; }

        [NotNull]
        public string PasswordHash { get; set; }

        public string RutaSelfie { get; set; }
    }
}

