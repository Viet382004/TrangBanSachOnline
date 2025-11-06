using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace TrangBanSachOnline.Models
{
    [Table("Order")]
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        public int OderStatusId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public OrderStatus? OderStatus { get; set; }
        [Required]
        public List<OrderDetail> OderDetail { get; set; }

    }
}
