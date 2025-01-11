using System.Text.Json.Serialization;

namespace WebCinema.Models
{
    public class Directors
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public bool IsDeleted { get; set; }
        public List<Movies>? Movie { get; set; }
    }
}