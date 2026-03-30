using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Dtos.RoleManagement
{
    public class ModuleRolePageDto
    {
        public int ModuleId { get; set; }
        public string ModuleDisplayName { get; set; } = "";
        public string ModuleAreaName { get; set; } = "";
        public List<RoleUserRowDto> AllUsers { get; set; } = new();
        public List<RolePermGroupDto> PermissionGroups { get; set; } = new();
    }
}
