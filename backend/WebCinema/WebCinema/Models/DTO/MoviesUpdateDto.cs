namespace WebCinema.Models.DTO
{
    public class MoviesUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public string Language { get; set; }
        public string AgeRating { get; set; }
        public int DirectorId { get; set; }
        public int CountryId { get; set; }
        public List<int> GenreIds { get; set; }
        public List<int> ActorIds { get; set; }
    }
}
