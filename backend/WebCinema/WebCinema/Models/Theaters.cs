using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCinema.Models
{
    public class Theaters
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public Cities City { get; set; }
        public string Adress { get; set; }
        public int PostalCode { get; set; }
        public int PhoneNumber { get; set; }
    }
}
