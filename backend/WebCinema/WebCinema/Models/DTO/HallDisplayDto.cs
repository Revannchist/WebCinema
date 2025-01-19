namespace WebCinema.Models.DTO
{
    public class HallDisplayDto
    {
        public int Id { get; set; }
        public int TheatersID { get; set; }
        public string TheaterName { get; set; }
        public string HallName { get; set; }
        public int Capacity { get; set; }
        public string HallType { get; set; }
    }
}
