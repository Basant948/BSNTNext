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
        public AuthServices(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

            var result = await _signInManager.PasswordSignInAsync(
                user,
                dto.Password,
                isPersistent: false,
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

        public async Task<Result> RegisterAsync(RegisterDto dto)
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
                CreatedAt   = DateTime.UtcNow
            };

            var identityResult = await _userManager.CreateAsync(user, dto.Password);

            if (!identityResult.Succeeded)
                return Result.Failure(identityResult.Errors.Select(e => e.Description));

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Result.Success("Registered successfully.");
        }
    }
}
