using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obligatorio.Models
{
    public class Favorito
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int ClienteId { get; set; }

        [NotNull]
        public int PeliculaId { get; set; }

        public string Titulo { get; set; }

        public string PosterUrl { get; set; }
    }
}

