using System.ComponentModel.DataAnnotations.Schema;

namespace WebCinema.Models
{
    public class MoviesGenres
    {
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movies Movie { get; set; }

        public int GenreId { get; set; }
        [ForeignKey("GenreId")]
        public Genres Genre { get; set; }
    }
}