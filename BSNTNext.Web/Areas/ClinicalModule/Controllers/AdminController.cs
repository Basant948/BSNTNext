using BSNTNext.Application.Interfaces.Services;
using BSNTNext.Infrastructure.Identity;
using BSNTNext.Infrastructure.Services;
using BSNTNext.Web.Infrastructure.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BSNTNext.Web.Areas.ClinicalModule.Controllers
{
    [Area("ClinicalModule")]
    public class AdminController : ModuleAdminBaseController
    {
        private readonly IModuleRoleService svc;
        private readonly UserManager<ApplicationUser> um;
        private readonly INavigationService nav;

        public AdminController(IModuleRoleService svc, UserManager<ApplicationUser> um, INavigationService nav)
            : base(svc, um, nav)
        {
            this.svc = svc;
            this.um = um;
            this.nav = nav;
        }

        protected override string AreaName => "ClinicalModule";
        protected override string ModuleTitle => "Clinical Module";
        protected override string ModuleIcon => "fa-solid fa-user-doctor";
        protected override string ModuleColor => "#198754";
        protected override string ToggleAccessUrl => "/ClinicalModule/Admin/ToggleAccess";
        protected override string GetPermissionsUrl => "/ClinicalModule/Admin/GetPermissions";
        protected override string SavePermissionsUrl => "/ClinicalModule/Admin/SavePermissions";

        [HttpGet]
        public async Task<IActionResult> RoleManagement()
        {
            if (!await PrepareRoleManagementViewBagAsync())
                return RedirectToAction("Login", "Account", new { area = "" });
            return View(await _svc.GetPageDataAsync(AreaName));
        }
    }
}