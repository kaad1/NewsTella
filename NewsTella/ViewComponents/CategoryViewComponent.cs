using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsTella.Data;

namespace NewsTella.ViewComponents
{
    public class CategoryViewComponent: ViewComponent
    {
        private AppDbContext _context;
        public CategoryViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await _context.Categories.ToListAsync();
            return View(item);
        }
    }
}
