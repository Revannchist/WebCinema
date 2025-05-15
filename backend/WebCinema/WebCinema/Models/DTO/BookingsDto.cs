namespace WebCinema.Models.DTO
{
    public class BookingsDto
    {
        public int Id { get; set; }
        public int ShowTimesId { get; set; }
        public string UserName { get; set; }
        public string MovieTitle { get; set; }
        public string HallName { get; set; }
        public DateTime ShowDateTime { get; set; }
        public DateTime BookingDate { get; set; }
        public int TicketQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string BookingStatus { get; set; }
        public List<int>? BookedSeats { get; set; }
    }
}
