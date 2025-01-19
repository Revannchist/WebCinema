using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models.DTO
{
    public class MovieCreateDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Duration { get; set; }

        [Required]
        [StringLength(50)]
        public string Language { get; set; }

        [Required]
        [StringLength(10)]
        public string AgeRating { get; set; }

        public int DirectorId { get; set; }

        public int CountryId { get; set; }

        public List<int> GenreIds { get; set; } = new List<int>();
        public List<int> ActorIds { get; set; } = new List<int>();
    }
}
