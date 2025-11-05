using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Configuration;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TrangBanSachOnline.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddItem(int bookId , int  qty = 1, int redirect =0)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để thêm sản phẩm." });
            }
            var cartCount =await _cartRepository.AddItem(bookId, qty);
            if (redirect ==0)
                return Json(cartCount);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> RemoveItem(int bookId)
        {
            var cartCount = await _cartRepository.RemoveItem(bookId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }
        public async Task<IActionResult> GetTotalItemInCart()
        {
            var cartItem = _cartRepository.GetCartItemCount();
            return Json(cartItem);
        }
    }
}
