using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obligatorio.Models
{
    [Table("Patrocinador")]
    public class Patrocinador
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100), NotNull]
        public string Nombre { get; set; }

        // Guardamos la ruta local del logo en el dispositivo (ejemplo: /storage/emulated/0/.../logo.png)
        [MaxLength(250)]
        public string LogoRuta { get; set; }

        // Dirección como texto, opcionalmente para mostrar en UI además del mapa
        [MaxLength(200)]
        public string Direccion { get; set; }

        // Latitud y Longitud de la ubicación en el mapa (geolocalización)
        [NotNull]
        public double Latitud { get; set; }

        [NotNull]
        public double Longitud { get; set; }
    }
}

