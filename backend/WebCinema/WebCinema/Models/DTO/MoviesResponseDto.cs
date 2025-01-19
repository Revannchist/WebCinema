namespace WebCinema.Models.DTO
{
    public class MoviesResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public string Language { get; set; }
        public string AgeRating { get; set; }
        public DirectorDto Director { get; set; }
        public CountryDto Country { get; set; }
        public List<GenreDto> Genres { get; set; }
        public List<ActorDto> Actors { get; set; }
    }
}