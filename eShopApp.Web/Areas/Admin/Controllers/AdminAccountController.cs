using AspNetCoreHero.ToastNotification.Abstractions;
using eShopApp.Models;
using eShopApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eShopApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _notifyService;
        public AdminAccountController(INotyfService notifyService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _notifyService = notifyService;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl= null)
        {
            ModelState.Remove("returnUrl");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    _notifyService.Success("Logged in successfully");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    _notifyService.Error("Invalid email or password privided.");
                }

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _notifyService.Information("Logged out successfully.");
            return RedirectToAction("Login", "AdminAccount", new { area = "Admin" });
        }
    }
}
