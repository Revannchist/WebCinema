using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebCinema.Models
{
    public class Bookings
    {
        public int Id { get; set; }

        public int UsersId { get; set; }
        [ForeignKey("UsersId")]
        [JsonIgnore]
        public Users? User { get; set; }

        public int ShowTimesId { get; set; }
        [ForeignKey("ShowTimesId")]
        [JsonIgnore]
        public ShowTimes? ShowTimes { get; set; }

        public int TicketQuantity { get; set; } 

        public decimal TotalPrice { get; set; } 

        public string BookingStatus { get; set; } = "Pending"; 

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        public ICollection<BookedSeats>? BookedSeats { get; set; }
    }

}
