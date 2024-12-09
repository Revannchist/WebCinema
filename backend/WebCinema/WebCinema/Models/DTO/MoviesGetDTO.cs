using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebCinema.Models.DTO
{
    public class MoviesGetDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; } //in minutes
        public string Language { get; set; }
        public string AgeRating { get; set; }

        public int DirectorId { get; set; }
        [ForeignKey("DirectorId")]

        [JsonIgnore]
        public Directors? Director { get; set; }

        public int CountryId { get; set; }
        [ForeignKey("CountryId")]

        [JsonIgnore]
        public Countries? Country { get; set; }

        public List<int>? MoviesGenresIds { get; set; }
        public List<int>? MoviesActorsIds { get; set; }
    }
}