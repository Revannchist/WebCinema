namespace WebCinema.Models.DTO
{
    public class BookingsResponseDto
    {
        public int Id { get; set; }
        public int UsersId { get; set; }
        public int ShowTimesId { get; set; }
        public DateTime BookingDate { get; set; }
        public int TicketQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string BookingStatus { get; set; }
        public List<int> BookedSeatsIds { get; set; } = new List<int>();
    }
}
