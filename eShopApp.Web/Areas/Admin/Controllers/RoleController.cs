using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
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
        public RoleController(RoleManager<IdentityRole> roleManager, INotyfService notifyService)
        {
            _notifyService = notifyService;
            _roleManager = roleManager;
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
    }
}
