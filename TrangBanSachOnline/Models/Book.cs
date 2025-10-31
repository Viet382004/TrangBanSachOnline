
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace TrangBanSachOnline.Models
{
    [Table("Book")]
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? BookName { get; set; }
        [Required]
        public double Price { get; set; }
        public string? Image { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
        public List<OderDetail>? OderDetail { get; set; }
        public List<CartDetail>? CartDetail { get; set; }
    }
}
