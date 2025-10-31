using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace TrangBanSachOnline.Models
{
    [Table("Oder")]
    public class Oder
    {
        public int Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        public int OderStatusId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public OderStatus? OderStatus { get; set; }
        [Required]
        public List<OderDetail> OderDetail { get; set; }

    }
}
