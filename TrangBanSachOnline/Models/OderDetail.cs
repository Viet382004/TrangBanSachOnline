using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace TrangBanSachOnline.Models
{
    [Table("OderDetail")]
    public class OderDetail
    {
        public int Id { get; set; }
        [Required]
        public int OderId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public Oder? Oder { get; set; }
        public Book? Book { get; set; }


    }
}
