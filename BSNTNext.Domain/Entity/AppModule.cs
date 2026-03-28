using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Domain.Entity
{
    public class AppModule
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;          // e.g. "AdminModule"
        public string DisplayName { get; set; } = string.Empty;   // e.g. "Admin"
        public string Description { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;       
        public string IconSvg { get; set; } = string.Empty;        
        public string BadgeColor { get; set; } = "#2A6EBB";
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<UserModuleAccess> UserAccesses { get; set; } = new List<UserModuleAccess>();
    }

}
