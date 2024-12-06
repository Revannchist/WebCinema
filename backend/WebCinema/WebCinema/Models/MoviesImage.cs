using System.ComponentModel.DataAnnotations.Schema;

namespace WebCinema.Models
{
    public class MoviesImage
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movies? Movies { get; set; }
        public byte[] ImageByteArray { get; set; }
        public string ImageFormat { get; set; }
    }
}
