using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TrangBanSachOnline.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TrangBanSachOnline.Models.Genre> Genres { get; set; }
        public DbSet<TrangBanSachOnline.Models.Book> Books { get; set; }
        public DbSet<TrangBanSachOnline.Models.OderDetail> OderDetails { get; set; }
        public DbSet<TrangBanSachOnline.Models.Oder> Oders { get; set; }
        public DbSet<TrangBanSachOnline.Models.OderStatus> OderStatues { get; set; }
        public DbSet<TrangBanSachOnline.Models.CartDetail> CartDetails { get; set;}
        public DbSet<TrangBanSachOnline.Models.ShoppingCart> ShoppingCarts { get; set;}
        


    }
}
