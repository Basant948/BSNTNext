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

            // ✅ Generate token and send the link built by the controller
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = verificationLink + $"?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendEmailVerificationAsync(user.Email, link);

            return Result.Success("Registration successful. Please check your email to confirm your account.");
        }
        public async Task<Result> ConfirmEmailAsync(Guid userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return Result.Failure("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                return Result.Failure("Invalid or expired token.");

            return Result.Success("Email confirmed successfully.");
        }
    }
}
