using BSNTNext.Application.Dtos.Auth;
using BSNTNext.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BSNTNext.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthServices _authServices;

        public AccountController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(dto);

            var result = await _authServices.LoginAsync(dto);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(dto);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
          // Post: /Account/Regisster
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto dto, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(dto);

            var confirmationBase = Url.Action("ConfirmEmail", "Account", null, Request.Scheme)!;

            var result = await _authServices.RegisterAsync(dto, confirmationBase);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? string.Join(", ", result.Errors));
                return View(dto);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction("RegisterConfirmation");
        }

        // RegistrationConfirmation
        [HttpGet]
        public IActionResult RegisterConfirmation()
        {
            ViewBag.Message = TempData["SuccessMessage"] ?? "Registration successful. Please check your email.";
            return View();
        }
          // Confirm email
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(Guid userId, string token)
        {
            if (userId == Guid.Empty || string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "Invalid confirmation link.";
                return View("Error");
            }

            token = Uri.UnescapeDataString(token);

            var result = await _authServices.ConfirmEmailAsync(userId, token);

            if (!result.Succeeded)
            {
                ViewBag.Error = result.Message;
                return View("Error");
            }

            return View("EmailConfirmed");
        }
        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authServices.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}