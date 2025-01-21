namespace WebCinema.Models.DTO
{
    public class TheaterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string Adress { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
