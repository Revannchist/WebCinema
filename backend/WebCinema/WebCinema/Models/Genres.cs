using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCinema.Models
{
    public class Genres
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MoviesGenres> MoviesGenres { get; set; }

        //[NotMapped] //nez treba li ovo
        //public int MoviesId { get; set; }
        //[ForeignKey("MoviesId")]
        //public Movies Movies { get; set; }
    }
}
