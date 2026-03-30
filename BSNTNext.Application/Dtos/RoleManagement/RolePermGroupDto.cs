using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Dtos.RoleManagement
{
    public class RolePermGroupDto
    {
        public string GroupName { get; set; } = "";
        public List<RolePermDto> Permissions { get; set; } = new();
    }
}
