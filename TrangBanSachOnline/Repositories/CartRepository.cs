using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Principal;
using TrangBanSachOnline.Models;

namespace TrangBanSachOnline.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, 
                              UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> AddItem(int bookId, int qty)
        {
            // Lấy userId từ context
            // Tạo giỏ hàng nếu chưa có
            // Thêm sách vào giỏ hàng
            string userId = GetUserId();
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("Người dùng chưa đăng nhập!");
                }

                var cart = await GetCart(userId);
                if (cart == null)
                {
                    cart = new ShoppingCart()
                    {
                        UserId = userId,
                    };
                    _db.ShoppingCarts.Add(cart);
                }
                // Lưu thay đổi để có Id của giỏ hàng
                // trước khi thêm mục giỏ hàng
                await _db.SaveChangesAsync();
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(cd => cd.BookId == bookId && cd.ShoppingCartId == cart.Id);
                if (cartItem != null)
                {
                    cartItem.Quantity += qty;
                    _db.CartDetails.Update(cartItem);
                }
                else
                {
                    cartItem = new CartDetail()
                    {
                        BookId = bookId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty
                    };
                    _db.CartDetails.Add(cartItem);
                }
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<int> RemoveItem(int bookId)
        {
            // Lấy userId từ context
            // Tạo giỏ hàng nếu chưa có
            // Thêm sách vào giỏ hàng
            // using var transaction = await _db.Database.BeginTransactionAsync();
            string userId = GetUserId();

            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("Người dùng chưa đăng nhập!");
                }
                var cart = await GetCart(userId);
                if (cart == null)
                {
                    throw new Exception("Giỏ hàng không hợp lệ!");
                }
                // Lưu thay đổi để có Id của giỏ hàng
                // trước khi thêm mục giỏ hàng
                await _db.SaveChangesAsync();
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(cd => cd.BookId == bookId && cd.ShoppingCartId == cart.Id);
                if (cartItem == null)
                {
                    throw new Exception("Không có sản phẩm nào trong giỏ hàng!");
                }
                else if(cartItem.Quantity == 1)
                {
                    _db.CartDetails.Remove(cartItem);
                }
                else 
                {
                    cartItem.Quantity -= 1;
                }
                await _db.SaveChangesAsync();
                //await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                //await transaction.RollbackAsync();
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if(userId == null)
            {
                throw new Exception("User not found");
            }
            var shoppingcart = await _db.ShoppingCarts
                                .Include(c => c.CartDetails)
                                .ThenInclude(cd => cd.Book)
                                .ThenInclude(b => b.Genre)
                                .Where(c => c.UserId == userId)
                                .FirstOrDefaultAsync();
            return shoppingcart;
        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId="")
        {
            if(!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from c in _db.ShoppingCarts
                                join cd in _db.CartDetails 
                                on c.Id equals cd.ShoppingCartId
                                select new
                                {
                                    cd.Id,
                                }).ToListAsync();
            return data.Count;
                                
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            return userId;
        }


    }
}
