using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;

namespace TrangBanSachOnline.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]

    public class StockController : Controller
    {
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }
        public async Task<IActionResult> Index(string sTerm = "")
        {
            var stocks = await _stockRepository.GetStocks(sTerm);
            return View(stocks);
        }
        public async Task<IActionResult> ManangeStock(int bookId)
        {
            var existingstock = await _stockRepository.GetStockByBookId(bookId);
            StockDTO stockDTO = new StockDTO()
            {
                BookId = bookId,
                Quantity = existingstock != null ? existingstock.Quantity : 0
            };
            return View(stockDTO);
        }
        [HttpPost]
        public async Task<IActionResult> ManangeStock(StockDTO stockToManage)
        {
            if (!ModelState.IsValid)
            {
                return View(stockToManage);
            }
            try
            {
                await _stockRepository.ManageStock(stockToManage);
                TempData["SuccessMessage"] = "Đã cập nhật kho thành công !";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi cập nhật kho: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
