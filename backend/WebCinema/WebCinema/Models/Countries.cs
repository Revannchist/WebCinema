using System.Text.Json.Serialization;

namespace WebCinema.Models
{
    public class Countries
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Movies>? Movie { get; set; }
    }
}
