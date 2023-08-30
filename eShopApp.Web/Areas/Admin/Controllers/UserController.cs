using AspNetCoreHero.ToastNotification.Abstractions;
using eShopApp.Models;
using eShopApp.Utility;
using eShopApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace eShopApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = UserRoles.AdminUser)]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotyfService _notifyService;
        public UserController(RoleManager<IdentityRole> roleManager, INotyfService notifyService, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _notifyService = notifyService;
            _roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<AdminUserViewModel> userList = new List<AdminUserViewModel>();
            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                AdminUserViewModel model = new AdminUserViewModel();

                model.UserDetails = new UserRegistrationViewModel()
                {
                    Address = user.Address,
                    City = user.City,
                    Name = user.Name,
                    Email = user.Email,
                    PostalCode = user.PostalCode,
                    State = user.State
                };
                model.Id = user.Id;
                model.Roles = (List<string>?)userRole;
                userList.Add(model);
            }
            return View(userList);
        }

        [HttpGet]
        public IActionResult ForgetPassword(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                _notifyService.Error("User not found");
                return View();
            }
            ResetPasswordViewModel model = new ResetPasswordViewModel()
            {
                ApplicationUserId = userId,
                Name = user.Name
            };
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> ForgetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var applicationUser = await _userManager.FindByIdAsync(model.ApplicationUserId);
            if (applicationUser == null)
            {
                _notifyService.Error("User details not found");
                return View(model);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

            var result = await _userManager.ResetPasswordAsync(applicationUser, token, model.Password);

            if (result.Succeeded)
            {
                _notifyService.Success("Password reset completed successfully.");
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }
            else
            {
                var error = result.Errors.FirstOrDefault();
                _notifyService.Error($"Failed to reset password: {error.Description}");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            AdminUserViewModel model = new AdminUserViewModel();
            SetUserRoles(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(AdminUserViewModel model)
        {
            SetUserRoles(model);
            if (!ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Email = model.UserDetails.Email,
                    UserName = model.UserDetails.Email,
                    Name = model.UserDetails.Name,
                    Address = model.UserDetails.Address,
                    City = model.UserDetails.City,
                    State = model.UserDetails.State,
                    PostalCode = model.UserDetails.PostalCode,
                };
                var result = await _userManager.CreateAsync(user, model.UserDetails.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.CurrentUserRole);
                    _notifyService.Success("New user created successfully");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Code + " " + error.Description);
                        _notifyService.Error("Error while creating user, {0}" + error.Description);
                        break;
                    }
                }
                return View(model);
            }
            return View();
        }

        private void SetUserRoles(AdminUserViewModel model)
        {
            var _roles = _roleManager.Roles.Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();

            _roles.Insert(0, new SelectListItem()
            {
                Text = "Select User Role",
                Value = ""
            });
            model.UserRoles = _roles;
        }
    }
}
