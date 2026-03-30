using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Dtos.RoleManagement
{
    public class RolePermDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = "";
        public string GroupName { get; set; } = "";
        public string IconClass { get; set; } = "";
        public string PermissionKey { get; set; } = "";
    }
}
