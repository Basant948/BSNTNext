using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Domain.Entity
{
    public class ModulePermission
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }
        public AppModule Module { get; set; } = null!;

        public string PermissionKey { get; set; } = string.Empty;   // e.g. "pharmacy.billing"
        public string DisplayName { get; set; } = string.Empty;     // e.g. "Billing"
        public string GroupName { get; set; } = string.Empty;       // e.g. "Finance"
        public string IconClass { get; set; } = string.Empty;       
        public string ControllerAction { get; set; } = string.Empty; // e.g. "Billing/Index"
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<UserModulePermission> UserPermissions { get; set; } = new List<UserModulePermission>();
    }
}