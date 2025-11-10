using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using TrangBanSachOnline.Models;
using TrangBanSachOnline.Models.DTOs;
using static Azure.Core.HttpHeader;

namespace TrangBanSachOnline.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepository;

        public GenreController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }
        public async Task<IActionResult> Index()
        {
            var genres = await _genreRepository.GetGenres();
            return View(genres);
        }
        public IActionResult AddGenre()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddGenre(GenreDTO genre)
        {
            if (!ModelState.IsValid)
            {
                return View(genre);
            }
            try
            {
                Genre newGenre = new Genre()
                {
                    GenreName = genre.GenreName,
                    Id = genre.Id
                };
                await _genreRepository.AddGenre(newGenre);
                TempData["SuccessMessage"] = "Đã thêm thể loại thành công !";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi thêm thể loại: {ex.Message}";
                return View(genre);
            }
        }
        public async Task<IActionResult> UpdateGenre(int id)
        {
            var genreToUpdate = await _genreRepository.GetGenreById(id);
            if (genreToUpdate is null)
                throw new InvalidOperationException($"Không tìm thấy thể loại với Id: {id}");
            var newGenre = new GenreDTO
            {
                Id = genreToUpdate!.Id,
                GenreName = genreToUpdate.GenreName
            };
            return View(newGenre);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateGenre(GenreDTO genre)
        {
            if (!ModelState.IsValid)
            {
                return View(genre);
            }
            try
            {
                Genre genreToUpdate = new Genre()
                {
                    Id = genre.Id,
                    GenreName = genre.GenreName
                };
                await _genreRepository.UpdateGenre(genreToUpdate);
                TempData["SuccessMessage"] = "Đã cập nhật thể loại thành công !";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi cập nhật thể loại: {ex.Message}";
                return View(genre);
            }
        }
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _genreRepository.GetGenreById(id);
            if (genre is null)
                throw new InvalidOperationException($"Không thấy thể loại mã: {id}");
            await _genreRepository.DeleteGenre(genre);
            return RedirectToAction(nameof(Index));

        }
    }
}
