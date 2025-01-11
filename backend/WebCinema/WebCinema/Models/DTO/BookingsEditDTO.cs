namespace WebCinema.Models.DTO
{
    public class BookingsEditDto
    {
        public ShowTimes? ShowTimes { get; set; }
        public DateTime BookingDateTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string BookingStatus { get; set; }
    }
}
