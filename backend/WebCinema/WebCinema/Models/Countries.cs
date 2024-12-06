namespace WebCinema.Models
{
    public class Countries
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Movies> Movie { get; set; }
    }
}
