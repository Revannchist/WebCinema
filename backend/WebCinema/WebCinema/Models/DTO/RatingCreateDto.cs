using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models.DTO
{
    public class RatingCreateDto
    {
        [Required]
        public int UsersId { get; set; }

        [Required]
        public int MoviesId { get; set; }

        [Required]
        public int Rating { get; set; }

        [StringLength(2000)]
        public string Review { get; set; }
    }
}
