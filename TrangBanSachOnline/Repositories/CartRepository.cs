using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("Người dùng chưa đăng nhập");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _db.ShoppingCarts.Add(cart);
                }
                _db.SaveChanges();
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var book = _db.Books.Find(bookId);
                    cartItem = new CartDetail
                    {
                        BookId = bookId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty,
                        UnitPrice = book.Price // this is new line after update
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<int> RemoveItem(int bookId)
        {
            //using var transaction = _db.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("Người dùng chưa đăng nhâp");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new InvalidOperationException("Giỏ hàng không hợp lệ");
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if (cartItem is null)
                    throw new InvalidOperationException("Không có sản phẩm nào trong giỏ hàng");
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if(userId == null)
            {
                throw new Exception("Không tìm thấy người dùng");
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
            if(string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }

            var data = await (from c in _db.ShoppingCarts
                                join cd in _db.CartDetails 
                                on c.Id equals cd.ShoppingCartId
                                where c.UserId == userId
                              select new{ cd.Id,}
                              ).ToListAsync();
            return data.Count;
                                
        }
        public async Task<bool> DoCheckout(CheckoutModel model)
        {
            var transaction = _db.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Người dùng chưa đăng nhập");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Giỏ hàng không hợp lệ");
                var cartDetails = _db.CartDetails
                                     .Where(a => a.ShoppingCartId == cart.Id)
                                     .ToList();
                if (cartDetails.Count == 0)
                    throw new Exception("Không có sản phẩm nào trong giỏ hàng");
                var pendingRecord = _db.OrderStatues.FirstOrDefault(os => os.StatusName.ToLower() == "Đang chờ xử lý");
                if (pendingRecord is null)
                    throw new Exception("Trạng thái đơn hàng không hợp lệ");
                var order = new Order
                {
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow,
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    PaymentMethod = model.PaymentMethod,
                    IsPaid = false,
                    OderStatusId = pendingRecord.Id,

                };
                _db.Orders.Add(order);
                _db.SaveChanges();
                foreach (var item in cartDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        OderId = order.Id,
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _db.OrderDetails.Add(orderDetail);
                }
                _db.SaveChanges();
                _db.CartDetails.RemoveRange(cartDetails);
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }


    }
}
