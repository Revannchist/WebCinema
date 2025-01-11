namespace WebCinema.Models.DTO
{
    public class ActorGetDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<MovieBriefDto> Movies { get; set; } = new List<MovieBriefDto>();
    }
}
