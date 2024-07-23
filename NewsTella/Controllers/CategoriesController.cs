using Microsoft.AspNetCore.Mvc;
using NewsTella.Data;
using NewsTella.Services;
using NewsTella.Models.ViewModel;

namespace NewsTella.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticlesService _articlesService;
		private readonly IStockService _stockService;

		public object? LatestLocal { get; private set; }

        public CategoriesController(ICategoryService categoryService, IArticlesService articlesService, IStockService stockService)
        {
            _categoryService = categoryService;
            _articlesService = articlesService;
            _stockService = stockService;
        } 

        public IActionResult Index()
        {
            return View();
        }

		public async Task <IActionResult> Articles(int categoryId)
        {
            var category = _categoryService.GetCategoryById(categoryId);
			if (category == null)
			{
				return NotFound();
			}
            category.Articles = _articlesService.GetPublishedArticlesByCategoryId(categoryId);

            if (categoryId == 4)
            {
                ViewBag.StockQuote = await _stockService.GetMostActiveStocksAsync();
            }
            
            return View(category);
        }

        public IActionResult LatestArticles()
        {
            var articleList = _articlesService.GetArticles();
            ArticleVM articleVM = new ArticleVM()
            {
                ArticleList = articleList.OrderByDescending(m => m.DateStamp).Take(3).ToList(),
            };
            return View();
        }
    }
}



		
		
		