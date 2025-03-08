namespace WebCinema.Models.DTO
{
    public class BookingsAddDto
    {
        public int UsersId { get; set; }
        public int ShowTimesId { get; set; }
        public List<int> BookedSeatsIds { get; set; } = new List<int>();
        public int TicketQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string BookingStatus { get; set; } = "Pending";
        public DateTime? BookingDate { get; set; }
    }
}
