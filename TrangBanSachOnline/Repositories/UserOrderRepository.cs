using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using TrangBanSachOnline.Data;
using static System.Net.Mime.MediaTypeNames;


namespace TrangBanSachOnline.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserOrderRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor
                                    , UserManager<IdentityUser> userManager)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task ChangOrderStatus(UpdateOrderStatusModel data)
        {
            var order = await _db.Orders.FindAsync(data.OrderId);
            if (order is null)
            {
                throw new Exception($"Đơn hàng mã : {data.OrderId} không tồn tại");
            }
            order.OderStatusId = data.OrderStatusId;
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _db.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return await _db.OrderStatues.ToListAsync();
        }

        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order is null)
            {
                throw new Exception($"Đơn hàng mã : {orderId} không tồn tại");
            }
            order.IsPaid = !order.IsPaid;
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> UserOrders(bool getAll = false)
        {
            var orders = _db.Orders
                            .Include(o => o.OderStatus)
                            .Include(o => o.OderDetail)
                            .ThenInclude(od => od.Book)
                            .ThenInclude(b => b.Genre)
                            .AsQueryable();
            if (!getAll)
            {
                var userId = GetUserId();
                if(string.IsNullOrEmpty(userId))
                {
                    throw new Exception("Người dùng chưa đăng nhập");
                }
                orders = orders.Where(o => o.UserId == userId);
                return await orders.ToListAsync();
            }
            return await orders.ToListAsync();
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
