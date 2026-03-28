using BSNTNext.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Infrastructure.Data.Seedings
{
    public static class ModuleSeeder
    {
        private const string SeedKey = "modules:v1";

        private static readonly ModuleDef[] Modules =
        {
        new(
            Name:        "AdminModule",
            DisplayName: "Admin",
            Description: "System administration and configuration",
            AreaName:    "AdminModule",
            SortOrder:   1,
            IconSvg:     """<i class="bi bi-gear-wide-connected"></i>"""
        ),
        new(
            Name:        "RegistrationModule",
            DisplayName: "Registration",
            Description: "Patient registration and management",
            AreaName:    "RegistrationModule",
            SortOrder:   2,
            IconSvg:     """<i class="bi bi-person-badge"></i>"""
        ),
        new(
            Name:        "ClinicalModule",
            DisplayName: "Clinical Module",
            Description: "Clinical records and patient care",
            AreaName:    "ClinicalModule",
            SortOrder:   3,
            IconSvg:     """<i class="bi bi-heart-pulse"></i>"""
        ),
        new(
            Name:        "PharmacyModule",
            DisplayName: "Pharmacy",
            Description: "Pharmacy and drug dispensing",
            AreaName:    "PharmacyModule",
            SortOrder:   4,
            IconSvg:     """<i class="bi bi-capsule"></i>"""
        ),
    };

        public static async Task SeedAsync(ApplicationDbContext db)
        {
            if (await db.SeedHistory.AnyAsync(s => s.SeedKey == SeedKey))  // ✅ fixed casing
                return;

            var existingAreas = await db.AppModules  // ✅ fixed DbSet name
                .Select(m => m.AreaName)
                .ToHashSetAsync();

            var toInsert = Modules
                .Where(m => !existingAreas.Contains(m.AreaName))
                .Select(m => new AppModule  // ✅ fixed type name
                {
                    Name = m.Name,
                    DisplayName = m.DisplayName,
                    Description = m.Description,
                    AreaName = m.AreaName,
                    SortOrder = m.SortOrder,
                    IconSvg = m.IconSvg,
                    IsActive = true
                })
                .ToList();

            if (toInsert.Count > 0)
                db.AppModules.AddRange(toInsert);  // ✅ fixed DbSet name

            db.SeedHistory.Add(new SeedHistory  // ✅ fixed casing
            {
                SeedKey = SeedKey,
                AppliedAt = DateTime.UtcNow,
                Notes = $"Seeded {toInsert.Count} module(s)"
            });

            await db.SaveChangesAsync();
        }

        private record ModuleDef(
            string Name,
            string DisplayName,
            string Description,
            string AreaName,
            int SortOrder,
            string IconSvg);
    }
}
