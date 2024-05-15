using Microsoft.AspNetCore.Mvc;

namespace NewsTella.Controllers
{
    public class SubscriptionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
