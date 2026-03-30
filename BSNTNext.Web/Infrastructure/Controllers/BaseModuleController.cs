using BSNTNext.Application.Interfaces.Services;
using BSNTNext.Infrastructure.Data;
using BSNTNext.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BSNTNext.Web.Infrastructure.Controllers
{
    [Authorize]
    public abstract class BaseModuleController : Controller
    {
        protected readonly ApplicationDbContext _db;
        protected readonly UserManager<ApplicationUser> _um;
        protected readonly INavigationService _nav;

        protected BaseModuleController(
            ApplicationDbContext db,
            UserManager<ApplicationUser> um,
            INavigationService nav)
        { _db = db; _um = um; _nav = nav; }

        protected abstract string AreaName { get; }

        protected virtual string ModuleTitle => AreaName;
        protected virtual string ModuleIcon => "bi bi-grid";
        protected virtual string ModuleColor => "#2A6EBB";
        protected virtual string RoleManagementUrl => $"/{AreaName}/Admin/RoleManagement";

        protected async Task SetNavAsync(string navKey)
        {
            var user = await _um.GetUserAsync(User);
            if (user == null) return;

            var isAdmin = await _um.IsInRoleAsync(user, "Admin");

            ViewBag.UserFullName = $"{user.FirstName} {user.LastName}".Trim();
            ViewBag.CurrentNavKey = navKey;
            ViewBag.ModuleTitle = ModuleTitle;
            ViewBag.ModuleIcon = ModuleIcon;
            ViewBag.ModuleColor = ModuleColor;
            ViewBag.RoleManagementUrl = RoleManagementUrl;

            await _nav.SetViewBagAsync(user.Id, isAdmin, AreaName, ViewBag);
        }
    }

}
    // ── SavePermissionsRequest ────────────────────────────────────────────────────

    public class SavePermissionsRequest
    {
        public Guid UserId { get; set; }
        public List<int> PermissionIds { get; set; } = new();
    }
