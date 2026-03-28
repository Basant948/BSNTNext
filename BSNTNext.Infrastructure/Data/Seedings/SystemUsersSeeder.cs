using BSNTNext.Domain.Entity;
using BSNTNext.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Infrastructure.Data.Seedings
{
    public class SystemUsersSeeder
    {
        private const string SeedKey = "admin-users:v1";

        private static readonly DefaultUser[] defaultUsers =
        {
                new(
                    FirstName: "BsntNext",
                    LastName: "Admin",
                    Email: "Admin@gmail.com",
                    Password: "Admin123",
                    Role: "Admin"
                )
        };

        public static async Task SeedAsync(
       ApplicationDbContext db,
       UserManager<ApplicationUser> userManager)
        {
            if (await db.seedHistory.AnyAsync(s => s.SeedKey == SeedKey))
                return; 

            int created = 0;

            foreach (var def in defaultUsers)
            {
                if (await userManager.FindByEmailAsync(def.Email) != null)
                    continue;

                var user = new ApplicationUser
                {
                    FirstName = def.FirstName,
                    LastName = def.LastName,
                    UserName = def.Email.ToLower(),
                    Email = def.Email.ToLower(),
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, def.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, def.Role);
                    created++;
                }
            }

            db.seedHistory.Add(new SeedHistory
            {
                SeedKey = SeedKey,
                AppliedAt = DateTime.UtcNow,
                Notes = $"Created {created} default user(s)"
            });

            await db.SaveChangesAsync();
        }
            private record DefaultUser(
                 string FirstName,
                 string LastName,
                 string Email,
                 string Password,
                 string Role);
    }
}
