using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TrangBanSachOnline.Data;
using TrangBanSachOnline.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TrangBanSachOnline.Repositories
{
    public class HomeRepository : iHomeRepository
    {
        public readonly ApplicationDbContext _db;
        public string STerm { get; set; }
        public int GenreId { get; set; }
        public HomeRepository(ApplicationDbContext db)
        {

            _db = db;
        }
        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _db.Genres.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Book> books = await (from b in _db.Books
                         join g in _db.Genres on b.GenreId equals g.Id
                         join s in _db.Stocks on b.Id equals s.BookId into bs
                         from stock in bs.DefaultIfEmpty()
                         where string.IsNullOrEmpty(sTerm) || (b != null && b.BookName.ToLower().Contains(sTerm))
                         select new Book
                         {
                             Id = b.Id,
                             BookName = b.BookName,
                             AuthorName = b.AuthorName,
                             Price = b.Price,
                             Image = b.Image,
                             GenreId = b.GenreId,
                             GenreName = g.GenreName,
                             Quantity = stock != null ? stock.Quantity : 0
                         }
                         ).ToListAsync();
            if (genreId > 0)
            {
                books = books.Where(b => b.GenreId == genreId).ToList();
            }
            return books;

        }
    }
}

