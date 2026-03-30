using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Dtos.RoleManagement
{
    public class RoleUserRowDto
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = "";
        public string Email { get; set; } = "";
        public bool HasAccess { get; set; }
    }
}
