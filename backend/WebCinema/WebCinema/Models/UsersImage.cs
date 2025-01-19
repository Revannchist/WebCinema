using System.ComponentModel.DataAnnotations.Schema;

namespace WebCinema.Models
{
    public class UsersImage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public Users? Users { get; set; }
        public byte[] ImageByteArray { get; set; }
        public string ImageFormat { get; set; }
    }
}
