using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Dtos.Auth
{
    public class RegisterDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;

        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
