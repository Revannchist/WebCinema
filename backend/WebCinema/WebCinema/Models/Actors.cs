using System.ComponentModel.DataAnnotations.Schema;

namespace WebCinema.Models
{
    public class Actors
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<MoviesActors> MoviesActors { get; set; }
    }
}
