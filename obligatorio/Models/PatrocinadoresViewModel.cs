using obligatorio.Data;
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
    public class PatrocinadoresViewModel : INotifyPropertyChanged
    {
        private readonly DataBaseService _databaseService;
        private bool _isLoading;
        private bool _isRefreshing;

        public PatrocinadoresViewModel()
        {
            _databaseService = App.Database;
            Patrocinadores = new ObservableCollection<Patrocinador>();
            InitializeAsync();
        }

        public ObservableCollection<Patrocinador> Patrocinadores { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotLoading));
            }
        }

        public bool IsNotLoading => !IsLoading;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        private async Task InitializeAsync()
        {
            await _databaseService.InitAsync();
        }

        public async Task LoadPatrocinadoresAsync()
        {
            try
            {
                IsLoading = true;

                var patrocinadores = await _databaseService.GetPatrocinadoresAsync();

                Patrocinadores.Clear();
                foreach (var patrocinador in patrocinadores)
                {
                    Patrocinadores.Add(patrocinador);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"No se pudieron cargar los patrocinadores: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
