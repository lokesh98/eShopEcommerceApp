using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using eShopApp.Models;
using eShopApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eShopApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotyfService _notifyService;
        private readonly UserManager<ApplicationUser> _userManager;
        public RoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, INotyfService notifyService)
        {
            _notifyService = notifyService;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var userRoles = _roleManager.Roles.Select(x => new RoleViewModel()
            {
                RoleId = x.Id,
                RoleName = x.Name,
            });
            return View(userRoles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoleAsync(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var _roles = await _roleManager.FindByNameAsync(model.RoleName);
            if (_roles == null)
            {
                var result =await  _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = model.RoleName,
                });
                if (result.Succeeded)
                {
                    _notifyService.Success("New role created successfully.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notifyService.Warning(result.Errors.FirstOrDefault()!.Description);
                }
            }
            else
            {
                _notifyService.Error("Role already exists.");
            }

            return View(model);
        }
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var _role = _roleManager.Roles.Where(x=>x.Id == roleId).FirstOrDefault();
            if (_role == null)
            {
                _notifyService.Warning("Role not found.");
                return RedirectToAction(nameof(Index));
            }
            var _userInRole =  await _userManager.GetUsersInRoleAsync(_role.Name);
            if (_userInRole == null)
            {
               await _roleManager.DeleteAsync(_role);
            }
            else
            {
                _notifyService.Warning("Role cannot be deleted since its assiged to diff users.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
