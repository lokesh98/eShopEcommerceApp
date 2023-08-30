using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using eShopApp.DataAccessLayer.UOF;
using eShopApp.Models;
using eShopApp.ViewModels;
using eShopApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;

namespace eShopApp.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly INotyfService _notifyService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<PaypalSetting> _paypalSettings;
        public CartController(IOptions<PaypalSetting> paypalSettings,IUnitOfWork unitOfWork, 
                                INotyfService notifyService, UserManager<ApplicationUser> userManager)
        {
            _notifyService = notifyService;
            _unitOfWork = unitOfWork;
            _userManager=userManager;
            _paypalSettings = paypalSettings;
        }
        public IActionResult Index()
        {
            var _claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = _claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartItemsViewModel viewModel = new CartItemsViewModel()
            {
                ListOfCartItems = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == claims.Value, includeProperties: "Product"),
                OrderHeader=new OrderHeader()
            };
            if (viewModel.ListOfCartItems.Count()==0)
            {
                return RedirectToAction("Index", "Home");
            }


            //viewModel.OrderHeader.OrderTotal = viewModel.ListOfCartItems.Sum(x => x.Product.Price * x.Count);
            foreach (var item in viewModel.ListOfCartItems)
            {
                viewModel.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
            }
            var currentUser = _userManager.FindByIdAsync(claims.Value).GetAwaiter().GetResult();
            if (currentUser!=null)
            {
                viewModel.OrderHeader.BillingAddress = currentUser.Address;
                viewModel.OrderHeader.BillingCity = currentUser.City;
                viewModel.OrderHeader.BillingState = currentUser.State;
                viewModel.OrderHeader.BillingPostalCode = currentUser.PostalCode;
                viewModel.OrderHeader.BillingName = currentUser.Name;
            }

            HttpContext.Session.SetString(claims.Value, JsonConvert.SerializeObject(viewModel));
            ViewBag.ClientId = _paypalSettings.Value.ClientId;
            return View(viewModel);
        }
    }
}
