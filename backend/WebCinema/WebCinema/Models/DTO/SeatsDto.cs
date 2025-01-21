namespace WebCinema.Models.DTO
{
    public class SeatsDto
    {
        public int Id { get; set; }
        public int HallsId { get; set; }
        public string HallName { get; set; }
        public int SeatNumber { get; set; }
        public string SeatType { get; set; }
    }
}
