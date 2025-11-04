using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

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
        public IActionResult AddItem(int bookId , int  qty = 1)
        {

            return View();
        }
        public IActionResult RemoveItem(int bookId)
        {
            return View();
        }
        public IActionResult GetUserCart()
        {
            return View();
        }
        public IActionResult GetTotalItemInCart(string userId)
        {
            return View();
        }
    }
}
