using System.ComponentModel.DataAnnotations.Schema;

namespace WebCinema.Models
{
    public class Booked_Seats
    {
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Bookings Bookings { get; set; }
        public int SeatsId { get; set; }
        [ForeignKey("SeatsId")]
        public Seats Seats { get; set; }
    }
}
