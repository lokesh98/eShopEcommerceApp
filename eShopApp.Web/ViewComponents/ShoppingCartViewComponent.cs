using eShopApp.DataAccessLayer.UOF;
using eShopApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eShopApp.Web.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public IViewComponentResult Invoke()
        {
            var _claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = _claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims == null)
            {
                HttpContext.Session.SetInt32("TotalCartItems", 0);
                HttpContext.Session.Clear();
                return View(0);
            }
            else
            {
                var cartCounter = HttpContext.Session.GetInt32("TotalCartItems");
                if (cartCounter!=null)
                {
                    return View(cartCounter.GetValueOrDefault());
                }
                else
                {
                    cartCounter = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == claims.Value, null).ToList().Count();
                    HttpContext.Session.SetInt32("TotalCartItems", cartCounter.GetValueOrDefault());
                    return View(cartCounter.GetValueOrDefault());
                }
            }
        }
    }
}
