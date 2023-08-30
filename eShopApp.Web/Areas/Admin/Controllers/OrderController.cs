using AspNetCoreHero.ToastNotification.Abstractions;
using eShopApp.DataAccessLayer.UOF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShopApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly INotyfService _notifyService;
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _notifyService = notifyService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var orders = _unitOfWork.OrderHeader.GetAll(null, includeProperties: "ApplicationUser");

            return View(orders);
        }
    }
}
