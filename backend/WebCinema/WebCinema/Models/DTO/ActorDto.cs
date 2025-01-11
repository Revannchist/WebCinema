namespace WebCinema.Models.DTO
{
    public class ActorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<MoviesActors>? MoviesActors { get; set; }
    }
}