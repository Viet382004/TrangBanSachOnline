using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace TrangBanSachOnline.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        public int Id { get; set; }
        [Required]
        public int ShoppingCartId { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
        [Required]
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int Quantity { get; set; }
    }
}
