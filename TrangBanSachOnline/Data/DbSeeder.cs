using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TrangBanSachOnline.Constants;

namespace TrangBanSachOnline.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();

            // adding some roles to db
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            //create admin  user
            var admin = new IdentityUser
            {
                UserName = "admin123",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb == null)
            {
                await userMgr.CreateAsync(admin,"Admin@123");
                await userMgr.AddToRoleAsync(admin,Roles.Admin.ToString());
            }
        
        }
    }
}
