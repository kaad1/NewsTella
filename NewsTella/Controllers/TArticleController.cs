using NewsTella.Data;
using NewsTella.Services;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace NewsTella.Controllers
{
	public class TArticleController : Controller
	{
		private readonly IArticlesService _articlesService;

		public TArticleController(IArticlesService articlesService)
		{
			_articlesService = articlesService;
		}
		public IActionResult Index()
		{
			return View(_articlesService.GetArticles());
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Article article)
		{
			if (ModelState.IsValid)
            {
				_articlesService.AddArticle(article);
				return RedirectToAction("Index");				
            }
            return View(article);

		}
	}
}
