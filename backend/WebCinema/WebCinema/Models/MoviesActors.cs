using System.ComponentModel.DataAnnotations.Schema;

namespace WebCinema.Models
{
    public class MoviesActors
    {
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movies Movie { get; set; }

        public int ActorId { get; set; }
        [ForeignKey("ActorId")]
        public Actors Actor { get; set; }
    }
}
