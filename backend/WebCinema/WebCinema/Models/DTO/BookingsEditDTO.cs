namespace WebCinema.Models.DTO
{
    public class BookingsEditDTO
    {
        public ShowTimes? ShowTimes { get; set; }
        public DateTime BookingDateTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string BookingStatus { get; set; }
    }
}
