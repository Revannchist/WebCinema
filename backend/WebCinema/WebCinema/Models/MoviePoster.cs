using System.ComponentModel.DataAnnotations.Schema;

namespace WebCinema.Models
{
    public class MoviePoster
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movies? Movies { get; set; }
        public byte[] PosterImage { get; set; }
        public string ImageFormat { get; set; }
    }
}