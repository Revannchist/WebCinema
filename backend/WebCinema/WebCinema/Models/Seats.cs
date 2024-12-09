using System.ComponentModel.DataAnnotations.Schema;

namespace WebCinema.Models
{
    public class Seats
    {
        public int Id { get; set; }
        public int HallsId { get; set; }
        [ForeignKey("HallsId")]
        public Halls? Hall { get; set; }
        public int SeatNumber { get; set; }
        public string SeatType { get; set; }
    }
}
