using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebCinema.Models
{
    public class Seats
    {
        public int Id { get; set; }
        public int HallsId { get; set; }
        [ForeignKey("HallsId")]

        [JsonIgnore]
        public Halls? Hall { get; set; }
        public int SeatNumber { get; set; }
        public string SeatType { get; set; }
        [JsonIgnore]
        public ICollection<BookedSeats>? Booked_Seats { get; set; }


    }
}
