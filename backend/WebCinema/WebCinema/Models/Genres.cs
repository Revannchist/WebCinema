using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models
{
    public class Genres
    {
        public int Id { get; set; }

        //[StringLength(50)]
        public string Name { get; set; }
    }
}
