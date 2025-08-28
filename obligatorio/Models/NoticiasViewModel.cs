using obligatorio.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace obligatorio.Models
{
    public class NoticiasViewModel
    {
        private readonly NewsService _newsService;
        private bool _isBusy;
        private string _filtro;
        public ICommand OpenArticleCommand { get; }
        public ICommand ShareArticleCommand { get; }

        public ObservableCollection<Noticia> Noticias { get; } = new();
        public ObservableCollection<Noticia> NoticiasFiltradas { get; } = new();
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public string Filtro
        {
            get => _filtro;
            set
            {
                if (_filtro != value)
                {
                    _filtro = value;
                    OnPropertyChanged();
                    FiltrarNoticias(_filtro);
                }
            }
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
            NoticiasFiltradas.Clear();
            try
            {
                var lista = await _newsService.ObtenerNoticiasAsync();
                foreach (var n in lista)
                {
                    Noticias.Add(n);
                    NoticiasFiltradas.Add(n); 
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void FiltrarNoticias(string searchText)
        {
            searchText = searchText?.ToLower() ?? "";

            var filtradas = string.IsNullOrWhiteSpace(searchText)
                ? Noticias
                : new ObservableCollection<Noticia>(Noticias
                    .Where(n => (n.Title?.ToLower().Contains(searchText) == true) ||
                                (n.Description?.ToLower().Contains(searchText) == true)));

            NoticiasFiltradas.Clear();
            foreach (var n in filtradas)
                NoticiasFiltradas.Add(n);
        }

             public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
