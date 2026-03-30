using BSNTNext.Application.Interfaces.Services;
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
        private readonly IHomeService _homeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IHomeService homeService, UserManager<ApplicationUser> userManager)
        {
            _homeService = homeService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var dashboard = await _homeService.GetDashboardAsync(user.Id.ToString());
            if (dashboard == null)
                return RedirectToAction("Login", "Account");

            ViewBag.UserFullName = dashboard.UserFullName;

            return View(dashboard.Modules);
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