using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using TrangBanSachOnline.Models;

namespace TrangBanSachOnline.Models
{
    [Table("Stock")] 
    public class Stock
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int Quantity { get; set; }
    }
}
