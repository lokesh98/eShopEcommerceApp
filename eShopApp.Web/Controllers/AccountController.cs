using AspNetCoreHero.ToastNotification.Abstractions;
using eShopApp.Models;
using eShopApp.Utility;
using eShopApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eShopApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _notifyService;
        public AccountController(INotyfService notifyService,SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager=signInManager;
            _userManager=userManager;
            _notifyService = notifyService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
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
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _notifyService.Error("Invalid email or password privided.");
                }

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Warning("Please input all the required field.");
                return View(model);
            }
            var user = new ApplicationUser()
            {
                Email = model.Email,
                UserName=model.Email,
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                State = model.State,
                PostalCode = model.PostalCode,
            };

            var result = await _userManager.CreateAsync(user,model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Customer);
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                _notifyService.Success("Account created successfully");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Code + " " + error.Description);
                }
            }
            return View(model);
            
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _notifyService.Information("Logged out successfully.");
            return RedirectToAction("Index", "Home", new {area=""});
        }
    }
}
