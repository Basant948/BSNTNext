using BSNTNext.Domain.Base;
using BSNTNext.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Infrastructure.Identity
{
    public class ApplicationUser  : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
