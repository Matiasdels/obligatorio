using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obligatorio.Models
{
    public class Patrocinador
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Imagen { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }

        public string Direccion { get; set; }
    }
}
