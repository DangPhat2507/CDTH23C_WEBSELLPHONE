using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace web_SellPhone.Models
{
    public static class SeedData
    {
        public static async Task SeedRoleUserAdminAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<NguoiDung>>();

            string[] roles = { "Admin", "User" };
            foreach (var r in roles)
                if (!await roleMgr.RoleExistsAsync(r))
                    await roleMgr.CreateAsync(new IdentityRole(r));

            var adminEmail = "admin@store.com";
            var admin = await userMgr.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new NguoiDung
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    HoTen = "Quan Tri",
                    DiaChi = "Admin", 
                    EmailConfirmed = true
                };
                await userMgr.CreateAsync(admin, "Admin123!");
                await userMgr.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
