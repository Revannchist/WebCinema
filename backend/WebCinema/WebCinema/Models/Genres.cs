using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebCinema.Models
{
    public class Genres
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MoviesGenres>? MoviesGenres { get; set; }
    }
}
