using BSNTNext.Application.Interfaces.Services;
using BSNTNext.Infrastructure.Identity;
using BSNTNext.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BSNTNext.Web.Infrastructure.Controllers
{
    [Authorize(Roles = "Admin")]
    public abstract class ModuleAdminBaseController : Controller
    {
        protected readonly IModuleRoleService _svc;
        protected readonly UserManager<ApplicationUser> _um;
        protected readonly INavigationService _nav;

        protected ModuleAdminBaseController(
            IModuleRoleService svc,
            UserManager<ApplicationUser> um,
            INavigationService nav)
        { _svc = svc; _um = um; _nav = nav; }

        protected abstract string AreaName { get; }
        protected abstract string ToggleAccessUrl { get; }
        protected abstract string GetPermissionsUrl { get; }
        protected abstract string SavePermissionsUrl { get; }

        protected virtual string ModuleTitle => AreaName;
        protected virtual string ModuleIcon => "bi bi-grid";
        protected virtual string ModuleColor => "#2A6EBB";

        protected async Task<bool> PrepareRoleManagementViewBagAsync()
        {
            var user = await _um.GetUserAsync(User);
            if (user == null) return false;

            ViewBag.UserFullName = $"{user.FirstName} {user.LastName}".Trim();
            ViewBag.CurrentNavKey = "admin.rolemgmt";
            ViewBag.IsAdmin = true;
            ViewBag.ToggleAccessUrl = ToggleAccessUrl;
            ViewBag.GetPermissionsUrl = GetPermissionsUrl;
            ViewBag.SavePermissionsUrl = SavePermissionsUrl;
            ViewBag.ModuleTitle = ModuleTitle;
            ViewBag.ModuleIcon = ModuleIcon;
            ViewBag.ModuleColor = ModuleColor;
            ViewBag.RoleManagementUrl = $"/{AreaName}/Admin/RoleManagement";
            ViewBag.ModuleNav = _nav.GetFullNavForArea(AreaName);

            return true;
        }

        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> ToggleAccess([FromBody] Guid userId)
        {
            var admin = await _um.GetUserAsync(User);
            if (admin == null) return Unauthorized();
            return Json(new { hasAccess = await _svc.ToggleModuleAccessAsync(AreaName, userId, admin.Id) });
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions(Guid userId) =>
            Json(await _svc.GetUserPermissionsAsync(AreaName, userId));

        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> SavePermissions([FromBody] SavePermissionsRequest req)
        {
            var admin = await _um.GetUserAsync(User);
            if (admin == null) return Unauthorized();
            await _svc.SaveUserPermissionsAsync(AreaName, req.UserId, req.PermissionIds, admin.Id);
            return Json(new { success = true });
        }
    }

}
