using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obligatorio.Models
{
    public class Movie
    {
        public string Title { get; set; }
        public string Overview { get; set; }
        public string PosterPath { get; set; }
        public double VoteAverage { get; set; } // Opcional para el rating
    }
}
