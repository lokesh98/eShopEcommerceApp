using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using eShopApp.DataAccessLayer.UOF;
using eShopApp.Models;
using eShopApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Xml;

namespace eShopApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notifyService;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork, INotyfService notifyService,ILogger<HomeController> logger)
        {
            _logger = logger;
            _notifyService = notifyService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ProductViewModel productView = new ProductViewModel();
            productView.ProductList = _unitOfWork.Product.GetAll(null, includeProperties: "Category");

            return View(productView);
        }
        [HttpGet]
        public IActionResult ViewProductDetails(int? productId)
        {
            if (productId==null)
            {
                _notifyService.Warning("Product ID not found");
                return RedirectToAction("Index", "Home");
            }
            var product = _unitOfWork.Product.GetT(x => x.ProductId == productId, includeProperties: "Category");
            if (product == null)
            {
                _notifyService.Error("Product not found");
                return RedirectToAction("Index", "Home");
            }
            CartItem cartItem = new CartItem()
            {
                Count = 1,
                Product = product
            };
            return View(cartItem);
        }
        [HttpPost]
        [Authorize]
        public IActionResult ViewProductDetails(CartItem cartItem)
        {
            ModelState.Remove("ApplicationUserId");
            if(ModelState.IsValid)
            {
                var _claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = _claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cartItem.ApplicationUserId = claims.Value;

                var items = _unitOfWork.Cart.GetT(x => x.ProductId == cartItem.ProductId && x.ApplicationUserId == cartItem.ApplicationUserId);
                if (items==null) //If not added in cart
                {
                    _unitOfWork.Cart.Add(cartItem);
                    _unitOfWork.Save();

                    int totalItems = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == cartItem.ApplicationUserId).Count();

                    HttpContext.Session.SetInt32("TotalCartItems", totalItems);
                }
                else //update cart
                {
                    _unitOfWork.Cart.IncrementCartItems(items, cartItem.Count);
                    _unitOfWork.Save();
                    int totalItems = _unitOfWork.Cart.GetAll(x => x.ApplicationUserId == cartItem.ApplicationUserId).Count();
                    HttpContext.Session.SetInt32("TotalCartItems", totalItems);
                }
                _notifyService.Success("Product added in cart successfully.");
                return RedirectToAction("Index", "Cart");

            }
            _notifyService.Error("Failed to add product in cart.");
            return View(cartItem);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}