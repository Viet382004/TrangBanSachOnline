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
                         where string.IsNullOrEmpty(sTerm) || (b != null && b.BookName.ToLower().StartsWith(sTerm))
                         select new Book
                         {
                             Id = b.Id,
                             BookName = b.BookName,
                             AuthorName = b.AuthorName,
                             Price = b.Price,
                             Image = b.Image,
                             GenreId = b.GenreId,
                             GenreName = g.GenreName
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

