using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Models;  
using Microsoft.AspNetCore.Identity;

namespace CustomIdentityApp
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "_Aa123456";
            string moderatorEmail = "moderator@m.com";
            string mpassword = "_Mm123456";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await roleManager.FindByNameAsync("moderator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("moderator"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                ApplicationUser admin = new ApplicationUser { Email = adminEmail, UserName = adminEmail, name = "Админ"};
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
            if (await userManager.FindByNameAsync(moderatorEmail) == null)
            {
                ApplicationUser mod = new ApplicationUser { Email = moderatorEmail, UserName = moderatorEmail, name = "Модератор" };
                IdentityResult result = await userManager.CreateAsync(mod, mpassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(mod, "moderator");
                }
            }
        }
    }
}