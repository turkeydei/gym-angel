using GymAngel.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymAngel.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            // 1. Seed Roles
            var roles = new[] { "Admin", "Staff", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Role { Name = role });
                }
            }

            // 2. Seed Admin
            if (await userManager.FindByEmailAsync("admin@gymangel.com") == null)
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@gymangel.com",
                    FullName = "System Admin",
                    CreatedAt = DateTime.UtcNow
                };

                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // 3. Seed Staff
            if (await userManager.FindByEmailAsync("staff1@gymangel.com") == null)
            {
                var staff = new User
                {
                    UserName = "staff1",
                    Email = "staff1@gymangel.com",
                    FullName = "Nguyen Van Staff",
                    CreatedAt = DateTime.UtcNow
                };

                await userManager.CreateAsync(staff, "Staff@123");
                await userManager.AddToRoleAsync(staff, "Staff");
            }

            // 4. Seed Customer
            if (await userManager.FindByEmailAsync("customer1@gymangel.com") == null)
            {
                var customer = new User
                {
                    UserName = "customer1",
                    Email = "customer1@gymangel.com",
                    FullName = "Nguyen Van A",
                    CreatedAt = DateTime.UtcNow,
                    MembershipStart = DateTime.UtcNow,
                    MembershipExpiry = DateTime.UtcNow.AddMonths(6)
                };

                await userManager.CreateAsync(customer, "Customer@123");
                await userManager.AddToRoleAsync(customer, "User");
            }
        }
    }
}
