using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace obligatorio.Models
{
    public class Movie : INotifyPropertyChanged
    {
        private bool _isLoadingPoster;

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonProperty("popularity")]
        public double Popularity { get; set; }

        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("video")]
        public bool Video { get; set; }

        [JsonProperty("genre_ids")]
        public int[] GenreIds { get; set; }

        public int Runtime { get; set; }
        public long Budget { get; set; }
        public long Revenue { get; set; }
        public string Director { get; set; }
        public bool HasTrailer { get; set; }
        public string TrailerKey { get; set; }
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public List<Cast> Cast { get; set; } = new List<Cast>();

        [JsonIgnore]
        public string PosterUrl => !string.IsNullOrEmpty(PosterPath)
            ? $"https://image.tmdb.org/t/p/w500{PosterPath}"
            : "poster_placeholder.png";

        [JsonIgnore]
        public string BackdropUrl => !string.IsNullOrEmpty(BackdropPath)
            ? $"https://image.tmdb.org/t/p/w1280{BackdropPath}"
            : "backdrop_placeholder.png";

        [JsonIgnore]
        public string TrailerUrl => HasTrailer && !string.IsNullOrEmpty(TrailerKey)
            ? $"https://www.youtube.com/watch?v={TrailerKey}"
            : string.Empty;



        [JsonIgnore]
        public bool IsLoadingPoster
        {
            get => _isLoadingPoster;
            set
            {
                _isLoadingPoster = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Genre
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Cast
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("character")]
        public string Character { get; set; }

        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }

        [JsonIgnore]
        public string ProfileUrl => !string.IsNullOrEmpty(ProfilePath)
            ? $"https://image.tmdb.org/t/p/w185{ProfilePath}"
            : "person_placeholder.png";
    }

 
}
