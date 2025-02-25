using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebCinema.Models
{
    public class ShowTimes
    {
        public int Id { get; set; }
        public int MoviesId { get; set; }
        [ForeignKey("MoviesId")]
        [JsonIgnore]
        public Movies? Movies { get; set; }
        public int HallsId { get; set; }
        [ForeignKey("HallsId")]
        [JsonIgnore]
        public Halls? Halls { get; set; }
        public DateTime ShowDateTime { get; set; }
        public decimal TicketPrice { get; set; }
    }
}
