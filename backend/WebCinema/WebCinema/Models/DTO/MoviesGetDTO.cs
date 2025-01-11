using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebCinema.Models.DTO
{
    public class MoviesGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; } //in minutes
        public string Language { get; set; }
        public string AgeRating { get; set; }
        public DirectorDto DirectorId { get; set; }
        public CountryDto CountryId { get; set; }
        public List<int> MoviesGenresIds { get; set; }
        public List<int> MoviesActorsIds { get; set; }
    }
}