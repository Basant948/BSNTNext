using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailVerificationAsync(string toEmail, string verificationLink);
        Task SendPasswordResetAsync(string toEmail, string resetLink);
    }
}
