using System;
using TrangBanSachOnline.Data;
using TrangBanSachOnline.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TrangBanSachOnline.Repositories
{
    public class StockRepository
    {
        private readonly ApplicationDbContext _db;

        public StockRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Stock?> GetStockByBookId(int bookId)
        => _db.Stocks.FirstOrDefault(s => s.BookId == bookId);
        
    }
}
