using obligatorio.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace obligatorio.Models
{
    public class NoticiasViewModel
    {
        private readonly NewsService _newsService;
        private bool _isBusy;

        public ObservableCollection<Noticia> Noticias { get; } = new();

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public NoticiasViewModel()
        {
            _newsService = new NewsService();
        }

        public async Task CargarNoticiasAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            Noticias.Clear();
            try
            {
                var lista = await _newsService.ObtenerNoticiasAsync();
                foreach (var n in lista)
                    Noticias.Add(n);
            }
            finally
            {
                IsBusy = false;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
