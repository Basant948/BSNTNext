using BSNTNext.Application.Interfaces.Services;
using BSNTNext.Infrastructure.Data;
using BSNTNext.Infrastructure.Identity;
using BSNTNext.Web.Infrastructure.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BSNTNext.Web.Areas.PharmacyModule.Controllers
{
    [Area("PharmacyModule")]
    public class DashboardController : BaseModuleController
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> um;
        private readonly INavigationService nav;

        public DashboardController(
            ApplicationDbContext db,
            UserManager<ApplicationUser> um,
            INavigationService nav)
            : base(db, um, nav)
        {
            this.db = db;
            this.um = um;
            this.nav = nav;
        }

        protected override string AreaName => "PharmacyModule";
        protected override string ModuleTitle => "Pharmacy Module";
        protected override string ModuleIcon => "fa-solid fa-user-doctor";
        protected override string ModuleColor => "#198754";

        public async Task<IActionResult> Index()
        {
            await SetNavAsync("pharmacy.dashboard");
            return View();
        }
    }
}