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
		public async Task<IActionResult> Create(Article article)
		{

			if (!ModelState.IsValid)
			{				
				ModelState.Clear(); // Rensa felmeddelanden
				return View("Index");
			}



			if (ModelState.IsValid)
			{
				_articlesService.AddArticle(article);
				return RedirectToAction("Index");
			}
			return View(article);

		}

		//[HttpPost]
		//public async Task<IActionResult> Create(Article article)
		//{

		//	//if (!ModelState.IsValid)
		//	//{ Clear Form
		//	//	ModelState.Clear(); // Rensa felmeddelanden
		//	//	return View("Index", model);
		//	//}



		//	article.Category = string.Join(", ", article.Cathegories);

		//	var file = article.FormImage;
		//	if (file != null && file.Length > 0)
		//	{
		//		var filePath = Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot\Images\", file.FileName);
		//		article.ImageLink = "/Images/"+ file.FileName;
		//		using (var stream = new FileStream(filePath, FileMode.Create))
		//		{
		//			await file.CopyToAsync(stream);
		//		}
		//	}
		//		//if (ModelState.IsValid)
		//		//{
		//		_articlesService.AddArticle(article);
		//		return RedirectToAction("Index");				
		//          //}
		//          //return View(article);

		//}	

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

        [HttpPost]
        public IActionResult DeleteDraft(Article article)
        {
			ModelState.Clear(); // Rensa felmeddelanden
								//	return View("Index", model);
			return RedirectToAction("Create");
        }
        public IActionResult Details(int id)
		{
			var article = _articlesService.GetArticlesById(id);
			return View(article);
		}
	}
}
