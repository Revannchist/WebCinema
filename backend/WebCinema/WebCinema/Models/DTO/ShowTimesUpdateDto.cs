using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models.DTO
{
    public class ShowTimesUpdateDto
    {
        [Required]
        public int MoviesId { get; set; }

        [Required]
        public int HallsId { get; set; }

        [Required]
        public DateTime ShowDateTime { get; set; }

        [Required]
        [Range(0.01, 999.99)]
        public decimal TicketPrice { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
