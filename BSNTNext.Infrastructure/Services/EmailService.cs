using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Infrastructure.Services
{
    using BSNTNext.Application.Common;
    using BSNTNext.Application.Interfaces.Services;
    using Microsoft.Extensions.Options;
    using System.Net;
    using System.Net.Mail;

    public class EmailService : IEmailService
    {
        private readonly GmailSettings _settings;

        public EmailService(IOptions<GmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailVerificationAsync(string toEmail, string verificationLink)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_settings.Email, "BsntNext"),
                Subject = "Email Verification",
                Body = $"<p>Please verify your email:</p><a href='{verificationLink}'>Verify Email</a>",
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            using var smtp = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Email, _settings.AppPassword),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
        }
        public async Task SendPasswordResetAsync(string toEmail, string resetLink)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_settings.Email, "BsntNext"),
                Subject = "Reset Your Password",
                Body = $"<p>You requested a password reset.</p>" +
                       $"<p><a href='{resetLink}'>Click here to reset your password</a></p>" +
                       $"<p>If you did not request this, ignore this email.</p>",
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            using var smtp = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Email, _settings.AppPassword),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
        }
    }
}
