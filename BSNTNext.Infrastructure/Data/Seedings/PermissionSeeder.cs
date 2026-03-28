using BSNTNext.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Infrastructure.Data.Seedings
{
    public static class PermissionSeeder
    {
        private static readonly Dictionary<string, string> VersionByArea = new()
        {
        };

        private static readonly Dictionary<string, PermDef[]> PermissionsByArea = new()
        {
            ["AdminModule"] = new[]
            {
            new PermDef("admin.dashboard", "Dashboard",       "General",  "bi bi-speedometer2",          "Dashboard/Index",  1),
            new PermDef("admin.settings",  "System Settings", "Settings", "bi bi-gear-wide-connected",   "Settings/Index",   2),
        },

            ["RegistrationModule"] = new[]
            {
            new PermDef("reg.dashboard",        "Dashboard",         "General",      "bi bi-speedometer2",    "Dashboard/Index",     1),
            new PermDef("reg.patient.register", "Register Patient",  "Patients",     "bi bi-person-plus",     "Patient/Register",    2),
            new PermDef("reg.patient.view",     "View Patients",     "Patients",     "bi bi-people",          "Patient/Index",       3),
            new PermDef("reg.appointment.view", "View Appointments", "Appointments", "bi bi-calendar3",       "Appointment/Index",   4),
            new PermDef("reg.appointment.book", "Book Appointment",  "Appointments", "bi bi-calendar-plus",   "Appointment/Book",    5),
        },

            ["ClinicalModule"] = new[]
            {
            new PermDef("clinical.dashboard",    "Dashboard",     "General", "bi bi-speedometer2",   "Dashboard/Index",    1),
            new PermDef("clinical.opd.view",     "OPD Queue",     "OPD",     "bi bi-list-check",     "Opd/Index",          2),
            new PermDef("clinical.opd.consult",  "Consultation",  "OPD",     "bi bi-stethoscope",    "Opd/Consult",        3),
            new PermDef("clinical.prescription", "Prescriptions", "OPD",     "bi bi-file-medical",   "Prescription/Index", 4),
        },

            ["PharmacyModule"] = new[]
            {
            new PermDef("pharmacy.dashboard",       "Dashboard",        "General",   "bi bi-speedometer2",       "Dashboard/Index",  1),
            new PermDef("pharmacy.items.view",      "View Items",       "Inventory", "bi bi-capsule",            "Items/Index",      2),
            new PermDef("pharmacy.items.manage",    "Manage Items",     "Inventory", "bi bi-pencil-square",      "Items/Manage",     3),
            new PermDef("pharmacy.stock.view",      "View Stock",       "Inventory", "bi bi-boxes",              "Stock/Index",      4),
            new PermDef("pharmacy.stock.update",    "Update Stock",     "Inventory", "bi bi-upload",             "Stock/Update",     5),
            new PermDef("pharmacy.billing.create",  "Create Bill",      "Billing",   "bi bi-file-earmark-text",  "Billing/Create",   6),
            new PermDef("pharmacy.billing.view",    "View Bills",       "Billing",   "bi bi-receipt",            "Billing/Index",    7),
            new PermDef("pharmacy.billing.cancel",  "Cancel Bill",      "Billing",   "bi bi-slash-circle",       "Billing/Cancel",   8),
            new PermDef("pharmacy.supplier.view",   "View Suppliers",   "Suppliers", "bi bi-truck",              "Supplier/Index",   9),
            new PermDef("pharmacy.supplier.manage", "Manage Suppliers", "Suppliers", "bi bi-truck-flatbed",      "Supplier/Manage",  10),
            new PermDef("pharmacy.reports.sales",   "Sales Report",     "Reports",   "bi bi-graph-up",           "Reports/Sales",    11),
            new PermDef("pharmacy.reports.stock",   "Stock Report",     "Reports",   "bi bi-bar-chart",          "Reports/Stock",    12),
        },
        };

        public static async Task SeedAsync(ApplicationDbContext db)
        {
            var modulesByArea = await db.AppModules  // ✅ fixed
                .ToDictionaryAsync(m => m.AreaName, m => m.Id);

            foreach (var (areaName, permDefs) in PermissionsByArea)
            {
                var version = VersionByArea.GetValueOrDefault(areaName, "v1");
                var seedKey = $"permissions:{areaName}:{version}";

                if (await db.SeedHistory.AnyAsync(s => s.SeedKey == seedKey))  // ✅ fixed
                    continue;

                if (!modulesByArea.TryGetValue(areaName, out var moduleId))
                    continue;

                var existingKeys = await db.ModulePermissions
                    .Where(p => p.ModuleId == moduleId)
                    .Select(p => p.PermissionKey)
                    .ToHashSetAsync();

                var toInsert = permDefs
                    .Where(p => !existingKeys.Contains(p.Key))
                    .Select(p => new ModulePermission
                    {
                        ModuleId = moduleId,
                        PermissionKey = p.Key,
                        DisplayName = p.DisplayName,
                        GroupName = p.GroupName,
                        IconClass = p.IconClass,
                        ControllerAction = p.ControllerAction,
                        SortOrder = p.SortOrder,
                        IsActive = true
                    })
                    .ToList();

                if (toInsert.Count > 0)
                    db.ModulePermissions.AddRange(toInsert);

                db.SeedHistory.Add(new SeedHistory  // ✅ fixed
                {
                    SeedKey = seedKey,
                    AppliedAt = DateTime.UtcNow,
                    Notes = $"Seeded {toInsert.Count} permission(s) for {areaName}"
                });

                await db.SaveChangesAsync();
            }
        }

        private record PermDef(
            string Key,
            string DisplayName,
            string GroupName,
            string IconClass,
            string ControllerAction,
            int SortOrder);
    }
}
