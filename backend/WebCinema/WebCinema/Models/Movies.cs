using System.ComponentModel.DataAnnotations.Schema;

namespace WebCinema.Models
{
    public class Movies
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public int Duration { get; set; } //in minutes
        public char Language { get; set; }
        public string AgeRating { get; set; }

        public int DirectorId { get; set; }
        [ForeignKey("DirectorId")]
        public Directors? Director { get; set; }

        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Countries? Country { get; set; }

        public ICollection<MoviesGenres> MoviesGenres { get; set; }

        public ICollection<MoviesActors> MoviesActors { get; set; }
    }
}