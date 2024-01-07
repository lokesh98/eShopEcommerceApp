using Microsoft.AspNetCore.Mvc;

namespace eShopApp.Web.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
