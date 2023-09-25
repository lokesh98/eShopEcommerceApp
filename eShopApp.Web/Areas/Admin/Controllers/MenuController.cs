﻿using eShopApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShopApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =UserRoles.AdminUser)]
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
