using BSNTNext.Application.Dtos.Home;
using BSNTNext.Application.Interfaces.Services;
using BSNTNext.Domain.Entity;
using BSNTNext.Infrastructure.Data;
using BSNTNext.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BSNTNext.Infrastructure.Services
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<DashboardResult?> GetDashboardAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            List<AppModule> modules;

            if (isAdmin)
            {
                modules = await _db.AppModules
                    .Where(m => m.IsActive)
                    .OrderBy(m => m.SortOrder)
                    .ToListAsync();
            }
            else
            {
                modules = await _db.UserModuleAccesses
                    .Where(a => a.UserId == user.Id)
                    .Include(a => a.Module)
                    .Where(a => a.Module.IsActive)
                    .Select(a => a.Module)
                    .OrderBy(m => m.SortOrder)
                    .ToListAsync();
            }

            return new DashboardResult
            {
                Modules = modules,
                UserFullName = $"{user.FirstName} {user.LastName}".Trim()
            };
        }
    }
}