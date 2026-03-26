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

        #region login  and logout

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authServices.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region register
        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

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

        #endregion

        #region registration and email confirmation

        [HttpGet]
        public IActionResult RegisterConfirmation()
        {
            ViewBag.Message = TempData["SuccessMessage"] ?? "Registration successful. Please check your email.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "Invalid confirmation link.";
                return View("Error");
            }

            token = Uri.UnescapeDataString(token);

            var result = await _authServices.ConfirmEmailAsync(userId, token);

            if (!result.Succeeded)
            {
                ViewBag.Error = result.Message;
                return View("ConfirmEmailFailed");
            }

            return View("EmailConfirmed");
        }

        #endregion


        #region password reset

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var resetBase = Url.Action("ResetPassword", "Account", null, Request.Scheme)!;

            var result = await _authServices.ForgotPasswordAsync(dto, resetBase);

            TempData["Message"] = result.Message;
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            ViewBag.Message = TempData["Message"] ?? "If this email is registered, a reset link has been sent.";
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "Invalid password reset link.";
                return View("Error");
            }

            var dto = new ResetPasswordDto
            {
                UserId = userId,
                Token = token
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _authServices.ResetPasswordAsync(dto);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? string.Join(", ", result.Errors));
                return View(dto);
            }

            TempData["Message"] = result.Message;
            return RedirectToAction("ResetPasswordConfirmed");
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmed()
        {
            ViewBag.Message = TempData["Message"] ?? "Password reset successful.";
            return View();
        }

        #endregion
    }
}