using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsTella.Data;
using NewsTella.Services;

namespace NewsTella.ViewComponents
{
    public class CategoryViewComponent: ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = _categoryService.GetCategories();
            return View(categories);
        }
    }
}
