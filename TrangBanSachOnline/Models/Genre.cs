using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace TrangBanSachOnline.Models
{
    [Table("Genre")]
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string GenreName { get; set; }

        public List<Book> Book { get; set; }
    }
}
