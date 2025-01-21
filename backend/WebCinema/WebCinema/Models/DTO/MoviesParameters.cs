namespace WebCinema.Models.DTO
{
    public class MoviesParameters
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        // Filter properties
        public string? SearchTerm { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Language { get; set; }
        public string? AgeRating { get; set; }
        public int? DirectorId { get; set; }
        public int? CountryId { get; set; }
        public List<int>? GenreIds { get; set; }
        public List<int>? ActorsIds { get; set; }

    }
}
