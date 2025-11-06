using System;
using TrangBanSachOnline.Data;
using static System.Net.Mime.MediaTypeNames;

namespace TrangBanSachOnline.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders();
    }
}