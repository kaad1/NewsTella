using Microsoft.AspNetCore.Mvc;
using NewsTella.Services;

namespace NewsTella.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        } 

        public IActionResult Index()
        {
            return View();
        }
        
        //public IActionResult Local()
        //{
        //    return View();
        //}

        public IActionResult Articles(int categoryId)
        {
            var category = _categoryService.GetCategoryById(categoryId);
            return View(category);
        }
    }
}
