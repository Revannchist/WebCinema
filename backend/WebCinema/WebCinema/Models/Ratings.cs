using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebCinema.Models
{
    public class Ratings
    {
        public int Id { get; set; }
        public int MoviesId { get; set; }
        [ForeignKey("MoviesId")]
        [JsonIgnore]
        public Movies? Movies { get; set; }
        public int UsersId { get; set; }
        [ForeignKey("UsersId")]
        [JsonIgnore]
        public Users? Users { get; set; }
        public int Rating { get; set; } //1to5
        public string Review { get; set; }
        public DateTime RatingDateTime { get; set; }
    }
}
