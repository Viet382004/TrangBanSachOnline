using Microsoft.EntityFrameworkCore;
using System;
using TrangBanSachOnline.Data;
using TrangBanSachOnline.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TrangBanSachOnline.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _db;

        public StockRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Stock?> GetStockByBookId(int bookId)
        => _db.Stocks.FirstOrDefault(s => s.BookId == bookId);

        public async Task ManageStock(StockDTO stockToManage)
        {
            // Check if stock exists for the given BookId
            // If it exists, update the quantity
            var existingStock = await GetStockByBookId(stockToManage.BookId);
            if (existingStock is null) 
            {
                var stock = new Stock
                {
                    BookId = stockToManage.BookId,
                    Quantity = stockToManage.Quantity
                };
                _db.Stocks.Add(stock);
            }
            else
            {
                existingStock.Quantity = stockToManage.Quantity;
            }
            await _db.SaveChangesAsync();
        }
        public async Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm="")
        {
            var stocks = await (from book in _db.Books
                                join stock in _db.Stocks on book.Id equals stock.BookId
                                into book_stock
                                from bookStock in book_stock.DefaultIfEmpty()
                                where string.IsNullOrWhiteSpace(sTerm) || 
                                book.BookName.ToLower().Contains(sTerm.ToLower())
                                select new StockDisplayModel
                                {
                                    BookId = book.Id,
                                    BookName = book.BookName,
                                    Quantity = bookStock == null ? 0 : bookStock.Quantity,
                                }).ToListAsync();
            return stocks;
        }
    }

    public interface IStockRepository
    {
        Task<Stock?> GetStockByBookId(int bookId);
        Task ManageStock(StockDTO stockToManage);
        Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm="");
    }
}
