using BookShoppingCartMvcUI.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.CodeDom;
using System.Threading.Tasks;
using TrangBanSachOnline.Models;
using TrangBanSachOnline.Models.DTOs;

namespace TrangBanSachOnline.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IFileService _fileService;

        public BookController(IBookRepository bookRepository,IGenreRepository genreRepository
                                                , IFileService fileService)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.GetBooks();
            return View(books);
        }
        public async Task<IActionResult> AddBook()
        {
            var getGenresList = (await _genreRepository.GetGenres()).Select(genre => new SelectListItem
            {
                Text = genre.GenreName,
                Value = genre.Id.ToString()
            });
            BookDTO bookToAdd = new ()
            {
                ListGenre = getGenresList
            };
            return View(bookToAdd);
        }
        [HttpPost]
        public async Task<IActionResult> AddBook(BookDTO bookToAdd)
        {
            var getGenresList = (await _genreRepository.GetGenres()).Select(genre => new SelectListItem
            {
                Text = genre.GenreName,
                Value = genre.Id.ToString()
            });
            bookToAdd.ListGenre = getGenresList;
            if(!ModelState.IsValid)
                return View(bookToAdd);
            try
            {
                if(bookToAdd.ImageFile != null)
                {
                    if(bookToAdd.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Dung lượng tối đa không quá 1MB.");
                        return View(bookToAdd);
                    }
                    string[] alowedExtensions = [".jpg",".jpeg",".png"];
                    string imageName = await _fileService.SaveFile(bookToAdd.ImageFile, alowedExtensions);
                    bookToAdd.Image = imageName;
                }
                // Map BookDTO to Book
                Book book = new()
                {
                    Id = bookToAdd.Id,
                    BookName = bookToAdd.BookName,
                    AuthorName = bookToAdd.AuthorName,
                    Price = (double)bookToAdd.Price,
                    Image = bookToAdd.Image,
                    GenreId = bookToAdd.GenreId
                };
                await _bookRepository.AddBook(book);
                TempData["Success"] = "Thêm sách thành công.";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = "Lỗi khi thêm sách: " + ex.Message;
                return View(bookToAdd);
            }
            catch (FileNotFoundException ex)
            {
                TempData["Error"] = "Lỗi khi thêm sách: " + ex.Message;
                return View(bookToAdd);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi thêm sách: " + ex.Message;
                return View(bookToAdd);
            }
        }
        public async Task<IActionResult> UpdateBook(int id) 
        {
            return View();
        }

    }
}
