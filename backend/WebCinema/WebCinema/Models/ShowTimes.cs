using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebCinema.Models
{
    public class ShowTimes
    {
        public int Id { get; set; }

        [Required]
        public int MoviesId { get; set; }

        [ForeignKey("MoviesId")]
        [JsonIgnore]
        public Movies? Movies { get; set; }

        [Required]
        public int HallsId { get; set; }

        [ForeignKey("HallsId")]
        [JsonIgnore]
        public Halls? Halls { get; set; }

        [Required]
        public DateTime ShowDateTime { get; set; }

        [Required]
        [Range(0.01, 999.99)]
        public decimal TicketPrice { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
