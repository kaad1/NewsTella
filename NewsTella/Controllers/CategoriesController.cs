using Microsoft.AspNetCore.Mvc;
using NewsTella.Data;
using NewsTella.Services;
using NewsTella.Models.Database;
using Microsoft.EntityFrameworkCore;

using NewsTella.Models;
using NewsTella.Models.ViewModel;
//using AspNetCore;

namespace NewsTella.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _dbcontext;
        private readonly IArticlesService _articlesService;
       

        public object? LatestLocal { get; private set; }

        public CategoriesController(ICategoryService categoryService, IArticlesService articlesService, AppDbContext dbcontext)
        {
            _categoryService = categoryService;
            _articlesService = articlesService;
            _dbcontext=dbcontext;
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
			if (category == null)
			{
				return NotFound();
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



		
		
		