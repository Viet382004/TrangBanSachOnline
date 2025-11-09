using Microsoft.AspNetCore.Mvc;

namespace TrangBanSachOnline.Controllers
{
    public class AdminOperations : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
