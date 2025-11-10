using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace TrangBanSachOnline.Models.DTOs
{
    public class GenreDTO
    {
        public int Id { get; set; }
        [Required]

        [MaxLength(40)]
        public string GenreName { get; set; } 
    }
}
