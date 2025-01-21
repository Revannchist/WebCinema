using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models.DTO
{
    public class RatingUpdateDto
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(2000)]
        public string Review { get; set; }
    }
}