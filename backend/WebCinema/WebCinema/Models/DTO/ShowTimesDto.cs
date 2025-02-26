using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models.DTO
{
    public class ShowTimesDto
    {
        public int Id { get; set; }

        [Required]
        public int MoviesId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string MovieTitle { get; set; } = string.Empty;

        [Required]
        public int HallsId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string HallName { get; set; } = string.Empty;

        [Required]
        public DateTime ShowDateTime { get; set; }

        [Required]
        [Range(0.01, 999.99)]
        public decimal TicketPrice { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
