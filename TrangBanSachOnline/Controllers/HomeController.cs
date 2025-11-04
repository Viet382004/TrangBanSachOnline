using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using TrangBanSachOnline.Models;
using TrangBanSachOnline.Models.DTOs;

namespace TrangBanSachOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly iHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, iHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string sTerm="",int genreId = 0)
        {
            IEnumerable<Book> books =  _homeRepository.GetBooks(sTerm,genreId).Result;
            IEnumerable<Genre> genres =  _homeRepository.Genres().Result;
            BookDisplayModel model = new BookDisplayModel()
            {
                Books = books,
                Genres = genres,
                STerm = sTerm,
                GenreId = genreId
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
