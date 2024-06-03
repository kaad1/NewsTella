using Microsoft.AspNetCore.Mvc;

namespace NewsTella.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Local()
        {
            return View();
        }

        public IActionResult Articles(int categoryId)
        {
            return View();
        }
    }
}
