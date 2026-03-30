using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Nav
{
    public static class NavRegistry
    {
        private static readonly Dictionary<string, ModuleNavDef> _modules = new()
        {
            ["AdminModule"] = new(
          PermKeyPrefix: "admin.",
          NavMap: new()
          {
              ["admin.dashboard"] = new("Dashboard", "bi bi-speedometer2", "/AdminModule/Dashboard/Index", "General"),
              ["admin.settings"] = new("System Settings", "bi bi-gear-wide-connected", "/AdminModule/Settings/Index", "Settings"),
          }
      ),

            ["RegistrationModule"] = new(
          PermKeyPrefix: "reg.",
          NavMap: new()
          {
              ["reg.dashboard"] = new("Dashboard", "bi bi-speedometer2", "/RegistrationModule/Dashboard/Index", "General"),
              ["reg.patient.register"] = new("Register Patient", "bi bi-person-plus", "/RegistrationModule/Patient/Register", "Patients"),
              ["reg.patient.view"] = new("View Patients", "bi bi-people", "/RegistrationModule/Patient/Index", "Patients"),
              ["reg.appointment.view"] = new("View Appointments", "bi bi-calendar3", "/RegistrationModule/Appointment/Index", "Appointments"),
              ["reg.appointment.book"] = new("Book Appointment", "bi bi-calendar-plus", "/RegistrationModule/Appointment/Book", "Appointments"),
          }
      ),

            ["ClinicalModule"] = new(
          PermKeyPrefix: "clinical.",
          NavMap: new()
          {
              ["clinical.dashboard"] = new("Dashboard", "bi bi-speedometer2", "/ClinicalModule/Dashboard/Index", "General"),
              ["clinical.opd.view"] = new("OPD Queue", "bi bi-list-check", "/ClinicalModule/Opd/Index", "OPD"),
              ["clinical.opd.consult"] = new("Consultation", "bi bi-stethoscope", "/ClinicalModule/Opd/Consult", "OPD"),
              ["clinical.prescription"] = new("Prescriptions", "bi bi-file-medical", "/ClinicalModule/Prescription/Index", "OPD"),
          }
      ),

            ["PharmacyModule"] = new(
          PermKeyPrefix: "pharmacy.",
          NavMap: new()
          {
              ["pharmacy.dashboard"] = new("Dashboard", "bi bi-speedometer2", "/PharmacyModule/Dashboard/Index", "General"),
              ["pharmacy.items.view"] = new("View Items", "bi bi-capsule", "/PharmacyModule/Items/Index", "Inventory"),
              ["pharmacy.items.manage"] = new("Manage Items", "bi bi-pencil-square", "/PharmacyModule/Items/Manage", "Inventory"),
              ["pharmacy.stock.view"] = new("View Stock", "bi bi-boxes", "/PharmacyModule/Stock/Index", "Inventory"),
              ["pharmacy.stock.update"] = new("Update Stock", "bi bi-upload", "/PharmacyModule/Stock/Update", "Inventory"),
              ["pharmacy.billing.create"] = new("Create Bill", "bi bi-file-earmark-text", "/PharmacyModule/Billing/Create", "Billing"),
              ["pharmacy.billing.view"] = new("View Bills", "bi bi-receipt", "/PharmacyModule/Billing/Index", "Billing"),
              ["pharmacy.billing.cancel"] = new("Cancel Bill", "bi bi-slash-circle", "/PharmacyModule/Billing/Cancel", "Billing"),
              ["pharmacy.supplier.view"] = new("View Suppliers", "bi bi-truck", "/PharmacyModule/Supplier/Index", "Suppliers"),
              ["pharmacy.supplier.manage"] = new("Manage Suppliers", "bi bi-truck-flatbed", "/PharmacyModule/Supplier/Manage", "Suppliers"),
              ["pharmacy.reports.sales"] = new("Sales Report", "bi bi-graph-up", "/PharmacyModule/Reports/Sales", "Reports"),
              ["pharmacy.reports.stock"] = new("Stock Report", "bi bi-bar-chart", "/PharmacyModule/Reports/Stock", "Reports"),
          }
      ),
        };

        public static IReadOnlyDictionary<string, NavItemDef>? GetMap(string areaName) =>
            _modules.TryGetValue(areaName, out var def) ? def.NavMap : null;
        public static string? GetPermKeyPrefix(string areaName) =>
            _modules.TryGetValue(areaName, out var def) ? def.PermKeyPrefix : null;

        public static IEnumerable<string> AreaNames => _modules.Keys;

        private record ModuleNavDef(
            string PermKeyPrefix,
            Dictionary<string, NavItemDef> NavMap);
    }
}
