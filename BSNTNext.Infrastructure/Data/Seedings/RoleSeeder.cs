using BSNTNext.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSNTNext.Infrastructure.Data.Seedings
{
    public static class RoleSeeder
    {
        private const string SeedKey = "roles:v1";

        private static readonly string[] Roles =
        {
            "Admin",
            "User"
        };

        public static async Task SeedAsync(
            ApplicationDbContext db,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            if (await db.seedHistory.AnyAsync(s => s.SeedKey == SeedKey))
                return;

            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            db.seedHistory.Add(new SeedHistory
            {
                SeedKey = SeedKey,
                AppliedAt = DateTime.UtcNow,
                Notes = $"Seeded {Roles.Length} role(s): {string.Join(", ", Roles)}"
            });

            await db.SaveChangesAsync();
        }
    }
}