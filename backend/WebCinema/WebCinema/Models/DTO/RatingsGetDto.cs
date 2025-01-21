namespace WebCinema.Models.DTO
{
    public class RatingsGetDto
    {
        public int Id { get; set; }
        public int MoviesId { get; set; }
        public int UsersId { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public DateTime RatingDateTime { get; set; }

        // Related Movie Info
        public string MovieTitle { get; set; }
        public string MovieDescription { get; set; }

        // Related User Info
        public string Username { get; set; }
        public string UserEmail { get; set; }
    }
}
