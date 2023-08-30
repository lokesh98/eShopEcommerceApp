using eShopApp.DataAccessLayer.UOF;
using Microsoft.AspNetCore.Mvc;

namespace eShopApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var totalEarning = _unitOfWork.OrderHeader.GetAll().Sum(x => x.OrderTotal);
            ViewBag.TotalEarning = totalEarning;
            return View();
        }
    }
}
