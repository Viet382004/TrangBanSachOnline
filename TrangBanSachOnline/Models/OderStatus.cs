using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace TrangBanSachOnline.Models
{
    [Table("OderStatus")]
    public class OderStatus
    {
        public int Id { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required,MaxLength(20)]
        public string ? StatusName { get; set; }
    }
}
