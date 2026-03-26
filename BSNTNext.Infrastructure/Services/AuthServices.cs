using BSNTNext.Application.Common;
using BSNTNext.Application.Dtos.Auth;
using BSNTNext.Application.Interfaces.Services;
using BSNTNext.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSNTNext.Infrastructure.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        public AuthServices(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        #region login and logout
        public async Task<Result> LoginAsync(LoginDto dto)
        {
            if (dto == null)
                return Result.Failure("Invalid request.");

            var email = dto.Email?.Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(dto.Password))
                return Result.Failure("Email and Password are required.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result.Failure("Invalid email or password.");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Result.Failure("Please confirm your email first.");

            var result = await _signInManager.PasswordSignInAsync(
                user,
                dto.Password,
                isPersistent: dto.RememberMe,
                lockoutOnFailure: true
            );

            if (result.IsLockedOut)
                return Result.Failure("User is locked out.");

            if (!result.Succeeded)
                return Result.Failure("Invalid email or password.");

            return Result.Success("Login successful.");
        }

        public async Task<Result> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return Result.Success("Logged out successfully.");
        }

        #endregion

        #region registration and email confirmation
        public async Task<Result> RegisterAsync(RegisterDto dto, string verificationLink)
        {
            if (dto == null)
                return Result.Failure("Invalid request.");

            var email = dto.Email?.Trim().ToLowerInvariant();
            var firstName = dto.FirstName?.Trim();
            var lastName = dto.LastName?.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(dto.Password))
                return Result.Failure("Email and Password are required.");

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
                return Result.Failure("User already exists.");

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.UtcNow
            };

            var identityResult = await _userManager.CreateAsync(user, dto.Password);
            if (!identityResult.Succeeded)
                return Result.Failure(identityResult.Errors.Select(e => e.Description));

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = verificationLink + $"?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendEmailVerificationAsync(user.Email, link);

            return Result.Success("Registration successful. Please check your email to confirm your account.");
        }
        public async Task<Result> ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId))
                return Result.Failure("Invalid request.");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Result.Failure("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                return Result.Failure("Invalid or expired token.");

            return Result.Success("Email confirmed successfully.");
        }
        #endregion

        #region password reset

        public async Task<Result> ForgotPasswordAsync(ForgotPasswordDto dto, string resetLink)
        {
            if (dto == null)
                return Result.Failure("Invalid request.");

            var email = dto.Email?.Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(email))
                return Result.Failure("Email is required.");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                return Result.Success("If this email is registered, a reset link has been sent.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = resetLink + $"?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendPasswordResetAsync(user.Email, link);

            return Result.Success("If this email is registered, a reset link has been sent.");
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (dto == null)
                return Result.Failure("Invalid request.");

            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
                return Result.Failure("Invalid request.");

            var token = Uri.UnescapeDataString(dto.Token);

            var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);

            if (!result.Succeeded)
                return Result.Failure(result.Errors.Select(e => e.Description));

            return Result.Success("Password reset successful. You can now log in.");
        }
        #endregion
    }
}
