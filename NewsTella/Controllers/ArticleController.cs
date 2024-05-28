using NewsTella.Data;
using NewsTella.Services;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

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
		//public IActionResult Create()
		//{
		//	var viewModel = new ArticleVM
		//	{
		//		Cathegories = AllCathegories.Select(c => new SelectListItem
		//		{
		//			Text = c,
		//			Value = c

		//		}).ToList()
		//	};
		//	return View(viewModel);
		//}

		public IActionResult Create()
		{			
			return View();
		}
		
		[HttpPost]
		public async Task<IActionResult> Create(Article article)
		{

			article.Category = string.Join(", ", article.Cathegories);

			var file = article.FormImage;
			if (file != null && file.Length > 0)
			{
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images\", file.FileName);
				article.ImageLink = "/Images/" + file.FileName;
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}
			}
			
			_articlesService.AddArticle(article);
			return RedirectToAction("Index");
			

		}
		//		public async Task<IActionResult> Edit(int Id)
		//		{
		//			var article = _articlesService.GetArticlesById(Id);
		//			if (article == null)
		//			{
		//				return NotFound();
		//			}


		//			var articleCategory =  _articlesService.GetArticlesById(Id);

		//			var model = new ArticleEditVM
		//			{
		//				ArticleId = article.Id,
		//				Category = article.Category,
		//				LinkText = article.LinkText,
		//				Headline = article.Headline,
		//				ContentSummary = article.ContentSummary,
		//				Content = article.Content,
		//				ImageLink = article.ImageLink, 
		//				Cathegories = _articlesService.Category.Select(c=>c.Cathegories).ToList(), 
		//				SelectedCathegories = articleCategory.ToList()
		//			};
		//			return View(model);
		//}

		public IActionResult Edit(int Id)
		{
			var article = _articlesService.GetArticlesById(Id);
			return View(article);
		}
		[HttpPost]
		public IActionResult Edit(Article article)
		{
			//if (ModelState.IsValid)
			//{
			_articlesService.UpdateArticle(article);
			return RedirectToAction("Index");
			//}
			//return View(article);
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
