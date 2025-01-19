using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebCinema.Models
{
    public class Halls
    {
        public int Id { get; set; }
        public int TheatersID { get; set; }
        [ForeignKey("TheatersID")]
        
        
        public Theaters? Theater { get; set; }
        public string HallName { get; set; }
        public int Capacity {  get; set; }
        public string HallType { get; set; }
    }
}
