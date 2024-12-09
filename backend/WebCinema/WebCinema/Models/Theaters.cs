using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebCinema.Models
{
    public class Theaters
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        [ForeignKey("CityId")]

        [JsonIgnore]
        public Cities? City { get; set; }
        public string Adress { get; set; }
        public int PostalCode { get; set; } //stavit da je string
        public int PhoneNumber { get; set; } //stavit da je string
    }
}