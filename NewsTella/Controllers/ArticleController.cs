using NewsTella.Data;
using NewsTella.Services;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace NewsTella.Controllers
{
	public class ArticleController : Controller
	{
		private readonly IArticlesService _articlesService;

		public ArticleController(IArticlesService articlesService)
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
			article.Category = article.Cathegories.FirstOrDefault();
			//article.Status = "Draft";
			//if (ModelState.IsValid)
            //{
				_articlesService.AddArticle(article);
				return RedirectToAction("Index");				
            //}
            //return View(article);

		}
		public IActionResult Edit(int Id)
		{
			var article = _articlesService.GetArticlesById(Id);
			return View(article);
		}
		[HttpPost]
		public IActionResult Edit(Article article)
		{
			if (ModelState.IsValid)
			{
				_articlesService.UpdateArticle(article);
				return RedirectToAction("Index");
			}
			return View(article);
		}
		public IActionResult Delete(int id)
		{
			var article = _articlesService.GetArticlesById(id);
			return View(article);
		}
		[HttpPost]
		public IActionResult Delete(Article article)
		{
			_articlesService.DeleteArticle(article);
			return RedirectToAction("Index");
		}
		public IActionResult Details(int id)
		{
			var article = _articlesService.GetArticlesById(id);
			return View(article);
		}
	}
}
