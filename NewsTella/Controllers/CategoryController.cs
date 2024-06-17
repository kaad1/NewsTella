using Microsoft.AspNetCore.Mvc;
using NewsTella.Data;
using NewsTella.Models.Database;
using NewsTella.Services;

namespace NewsTella.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly AppDbContext _context;

		public CategoryController(ICategoryService categoryService,
										  AppDbContext context)
		{
			_categoryService = categoryService;
			_context = context;
		}

		public IActionResult Index()
		{
			return View(_categoryService.GetCategories());
		}

        public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Category category)
		{
			ModelState.Remove("Articles");
			if (ModelState.IsValid)
			{
				_categoryService.AddCategory(category);
				return RedirectToAction(nameof(Index));
			}
			return View();
		}

		public IActionResult Delete(int id)
		{
			var category = _categoryService.GetCategoryById(id);
			return View(category);
		}
		[HttpPost]
		public IActionResult DeleteConfirmed(Category category)
		{
			category = _categoryService.GetCategoryById(category.Id);
			category.IsDeleted = true;
			_categoryService.UpdateCategory(category);
			return RedirectToAction(nameof(Index));
		}
		public IActionResult Edit(int id)
		{
			var category = _categoryService.GetCategoryById(id);
			return View(category);
		}

		[HttpPost]
		public IActionResult Edit(Category category)
		{
			_categoryService.UpdateCategory(category);
			return RedirectToAction(nameof(Index));
		}
	}
}
