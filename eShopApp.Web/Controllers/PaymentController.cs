using AspNetCoreHero.ToastNotification.Abstractions;
using eShopApp.DataAccessLayer.UOF;
using eShopApp.Models;
using eShopApp.Utility;
using eShopApp.ViewModels;
using eShopApp.Web.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace eShopApp.Web.Controllers
{
    
    public class PaymentController : Controller
    {
        private readonly PaypalClient _paypalClient;
        private readonly INotyfService _notifyService;
        private readonly IUnitOfWork _unitOfWork;
        public PaymentController(IUnitOfWork unitOfWork, PaypalClient paypalClient, INotyfService notifyService)
        {
            _paypalClient = paypalClient;
            _notifyService= notifyService;
            _unitOfWork= unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProceedPayment(CancellationToken cancellationToken)
        {
            try
            {
                var _claimsIdentity = (ClaimsIdentity)User.Identity!;
                var claims = _claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                var orderInfo = JsonConvert.DeserializeObject<CartItemsViewModel>(HttpContext.Session.GetString(claims.Value));

                string transactionID = Guid.NewGuid().ToString();
                var price = orderInfo!.OrderHeader.OrderTotal;
                var currency = "USD";
                var response = await _paypalClient.CreateOrder(price.ToString(), currency, transactionID);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CapturePayment(string orderID, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);

               
                var _claimsIdentity = (ClaimsIdentity)User.Identity!;
                var claims = _claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                var orderInfo = JsonConvert.DeserializeObject<CartItemsViewModel>(HttpContext.Session.GetString(claims.Value));

                //Save To OrderHeader
                PostOrderToDatabase(response, claims, orderInfo);

                //Remove items from cart
                var cartItems = _unitOfWork.Cart.GetAll(x=>x.ApplicationUserId == claims.Value);
                if (cartItems!=null)
                {
                    _unitOfWork.Cart.DeleteRange(cartItems);
                    _unitOfWork.Save();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Success(string orderId)
        {
            _notifyService.Success("Order placed successfully.");
            HttpContext.Session.SetInt32("TotalCartItems", 0);
            return View();
        }


        private void PostOrderToDatabase(CaptureOrderResponse response, Claim? claims, CartItemsViewModel? orderInfo)
        {
            //Save To OrderHeader
            orderInfo.OrderHeader.TransactionIDRef = response.purchase_units[0].reference_id;
            orderInfo.OrderHeader.TransactionID = response.purchase_units[0].payments.captures[0].id;

            orderInfo.OrderHeader.OrderDate = DateTime.Now;
            orderInfo.OrderHeader.ShippingDate = DateTime.Today.AddDays(6);
            orderInfo.OrderHeader.ApplicationUserId = claims.Value;
            orderInfo.OrderHeader.PaymentStatus = PaymentStatus.StatusApproved;
            orderInfo.OrderHeader.OrderStatus = OrderStatus.StatusApproved;

            _unitOfWork.OrderHeader.Add(orderInfo.OrderHeader);
            _unitOfWork.Save();
            //Step 2: Save To OrderDetails
            foreach (var item in orderInfo.ListOfCartItems)
            {
                OrderDetail detail = new OrderDetail()
                {
                    OrderHeaderId = orderInfo.OrderHeader.Id,
                    ProductId = item.ProductId,
                    Price = item.Product.Price,
                    TotalItemCounter = item.Count
                };
                _unitOfWork.OrderDetail.Add(detail);
            };
            _unitOfWork.Save();
        }

    }
}
