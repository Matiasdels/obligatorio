using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obligatorio.Models
{
    public class Usuario
    {


        
        
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            [MaxLength(100), NotNull]
            public string Nombre { get; set; }

            [MaxLength(100), Unique, NotNull]
            public string Email { get; set; }

            [MaxLength(100), NotNull]
            public string Password { get; set; }

            [MaxLength(50)]
            public string Rol { get; set; }

            // Para guardar la foto como Base64
            public string FotoBase64 { get; set; }

            // Opcional: dirección y teléfono si quieres almacenarlos en la tabla Usuario
            [MaxLength(200)]
            public string Direccion { get; set; }

            [MaxLength(20)]
            public string Telefono { get; set; }
        
    }
}
