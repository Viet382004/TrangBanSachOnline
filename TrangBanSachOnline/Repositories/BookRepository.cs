using Microsoft.EntityFrameworkCore;

namespace TrangBanSachOnline.Repositories
{
    public interface IBookRepository
    {
        Task AddBook(Book book);
        Task UpdateBook (Book book);
        Task DeleteBook(Book book);
        Task<Book?> GetBookById(int id);
        Task<IEnumerable<Book>> GetBooks();
    }
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _applicationDbContext1;
        public BookRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext1 = applicationDbContext;
        }

        public Task AddBook(Book book)
        {
            _applicationDbContext1.Books.Add(book);
            return _applicationDbContext1.SaveChangesAsync();
        }

        public Task DeleteBook(Book book)
        {
            _applicationDbContext1.Books.Remove(book);
            return _applicationDbContext1.SaveChangesAsync();
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _applicationDbContext1.Books.FindAsync(id);
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _applicationDbContext1.Books.Include(s => s.Genre).ToListAsync();
        }

        public Task UpdateBook(Book book)
        {
            _applicationDbContext1.Books.Update(book);
            return _applicationDbContext1.SaveChangesAsync();
        }
    }
}
