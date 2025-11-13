using BookShoppingCartMvcUI.Shared;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.CodeDom;
using System.Threading.Tasks;
using TrangBanSachOnline.Models;
using TrangBanSachOnline.Models.DTOs;
using static Azure.Core.HttpHeader;

namespace TrangBanSachOnline.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IFileService _fileService;

        public BookController(IBookRepository bookRepository, IGenreRepository genreRepository
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
            BookDTO bookToAdd = new()
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
            if (!ModelState.IsValid)
                return View(bookToAdd);
            try
            {
                if (bookToAdd.ImageFile != null)
                {
                    if (bookToAdd.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Dung lượng tối đa không quá 1MB.");
                        return View(bookToAdd);
                    }
                    string[] alowedExtensions = [".jpg", ".jpeg", ".png"];
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
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
            {
                TempData["errorMessage"] = $"Không tìm thấy cuốn sách với id : {id}";
                return RedirectToAction(nameof(Index));
            }
            var genrSelectList = (await _genreRepository.GetGenres()).Select(genre => new SelectListItem
            {
                Text = genre.GenreName,
                Value = genre.Id.ToString(),
                Selected = genre.Id == book.GenreId
            });
            BookDTO bookToUpdate = new()
            {
                ListGenre = genrSelectList,
                Id = book.Id,
                BookName = book.BookName,
                AuthorName = book.AuthorName,
                GenreId = book.GenreId,
                Price = book.Price,
                Image = book.Image
            };

            return View(bookToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBook(BookDTO bookToUpdate)
        {
            var getGenresList = (await _genreRepository.GetGenres()).Select(genre => new SelectListItem
            {
                Text = genre.GenreName,
                Value = genre.Id.ToString(),
                Selected = genre.Id == bookToUpdate.GenreId
            });
            bookToUpdate.ListGenre = getGenresList;
            if (!ModelState.IsValid)
                return View(bookToUpdate);
            try
            {
                string oldImage = "";
                if (bookToUpdate.ImageFile != null)
                {
                    if (bookToUpdate.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Dung lượng tối đa không quá 1MB.");
                        return View(bookToUpdate);
                    }
                    string[] alowedExtensions = [".jpg", ".jpeg", ".png"];
                    string imageName = await _fileService.SaveFile(bookToUpdate.ImageFile, alowedExtensions);
                    oldImage = bookToUpdate.Image!;
                    bookToUpdate.Image = imageName;
                }
                // Map BookDTO to Book
                Book book = new()
                {
                    Id = bookToUpdate.Id,
                    BookName = bookToUpdate.BookName,
                    AuthorName = bookToUpdate.AuthorName,
                    Price = (double)bookToUpdate.Price,
                    Image = bookToUpdate.Image,
                    GenreId = bookToUpdate.GenreId
                };
                await _bookRepository.UpdateBook(book);
                if (!string.IsNullOrWhiteSpace(oldImage))
                {
                    _fileService.DeleteFile(oldImage);
                }
                TempData["Success"] = "Cập nhật sách thành công.";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật sách: " + ex.Message;
                return View(bookToUpdate);
            }
            catch (FileNotFoundException ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật sách: " + ex.Message;
                return View(bookToUpdate);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật sách: " + ex.Message;
                return View(bookToUpdate);
            }
        }

        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var bookToDelete = await _bookRepository.GetBookById(id);
                if (bookToDelete == null)
                {
                    TempData["errorMessage"] = $"Không tìm thấy cuốn sách với id : {id}";
                }
                else
                {
                    await _bookRepository.DeleteBook(bookToDelete);
                    if(!string.IsNullOrWhiteSpace(bookToDelete.Image))
                    {
                        _fileService.DeleteFile(bookToDelete.Image);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = "Lỗi khi xóa sách: " + ex.Message;
            }
            catch (FileNotFoundException ex)
            {
                TempData["Error"] = "Lỗi khi xóa sách: " + ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa sách: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
