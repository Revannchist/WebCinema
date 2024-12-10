using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebCinema.Models
{
    public class Payments
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        [JsonIgnore]
        public Bookings? Bookings { get; set; }
        public string PaymentMethod { get; set; }
        public int TransactionID { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDateTime { get; set; }

    }
}
