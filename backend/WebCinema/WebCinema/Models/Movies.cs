using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebCinema.Models
{
    public class Movies
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; } //in minutes
        public string Language { get; set; }
        public string AgeRating { get; set; }

        public int? DirectorId { get; set; }
        [ForeignKey("DirectorId")]

        public Directors? Director { get; set; }

        public int? CountryId { get; set; }
        [ForeignKey("CountryId")]

        public Countries Country { get; set; }

        public List<MoviesGenres>? MoviesGenres { get; set; }

        public List<MoviesActors>? MoviesActors { get; set; }

        //public int? RatingsId { get; set; }
        //public Ratings Ratings { get; set; }  //i ovo treba kasnije implementovat
    }
}