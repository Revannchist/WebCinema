namespace WebCinema.Models.DTO
{
    public class RatingsResponseDto
    {
        public int Id { get; set; }
        public int UsersId { get; set; }
        public int MoviesId { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public DateTime RatingDateTime { get; set; }

        //public UsersDto User { get; set; } //dodat kasnije kad ramic uradit Usere i Dto
        public MoviesResponseDto Movie { get; set; }
    }
}
