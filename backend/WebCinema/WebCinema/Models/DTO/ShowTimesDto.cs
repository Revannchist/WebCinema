namespace WebCinema.Models.DTO
{
    public class ShowTimesDto
    {
        public int Id { get; set; }
        public int MoviesId { get; set; }
        public string MovieTitle { get; set; }
        public int HallsId { get; set; }
        public string HallName { get; set; }
        public DateTime ShowDateTieme { get; set; }
        public decimal TicketPrice { get; set; }
    }
}
