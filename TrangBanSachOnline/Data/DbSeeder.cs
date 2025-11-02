using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace TrangBanSachOnline.Data
{
    public static class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Tạo role
            string[] roleNames = { "Admin", "Customer" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Tạo user admin
            var adminEmail = "admin@gmail.com";
            var adminPassword = "Admin@123";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine("Tạo tài khoản Admin thành công !");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($" {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Tài khoản Admin đã tồn tại.");
            }
        }
    }
}
