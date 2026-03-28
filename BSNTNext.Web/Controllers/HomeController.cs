using BSNTNext.Domain.Entity;
using BSNTNext.Infrastructure.Data;
using BSNTNext.Infrastructure.Identity;
using BSNTNext.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BSNTNext.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            List<AppModule> modules;

            if (isAdmin)
            {
                modules = await _db.AppModules  // ✅ fixed
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

            ViewBag.UserFullName = $"{user.FirstName} {user.LastName}".Trim();  // ✅ fixed
            ViewBag.IsAdmin = isAdmin;
            return View(modules);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}