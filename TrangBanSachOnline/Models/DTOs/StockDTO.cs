using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrangBanSachOnline.Models.DTOs
{
    public class StockDTO
    {
        public int BookId { get; set; }
        [Range(0,int.MaxValue, ErrorMessage = "Số lượng sản phẩm trong kho không được để trống !")]
        public int Quantity { get; set; }
    }
}
