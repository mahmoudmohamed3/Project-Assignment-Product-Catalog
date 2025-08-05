﻿using Microsoft.AspNetCore.Identity;

namespace Product_Catalog.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser admin = new()
            {
                UserName = "admin",
                Email = "admin@product.com",
                FullName = "Admin",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(admin.Email);

            if(user is null)
            {
                await userManager.CreateAsync(admin, "P@ssword123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}