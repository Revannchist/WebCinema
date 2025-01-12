namespace WebCinema.Models.DTO
{
    public class MoviesImageDto
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string Image { get; set; }
        public bool IsPoster { get; set; }
    }
}
