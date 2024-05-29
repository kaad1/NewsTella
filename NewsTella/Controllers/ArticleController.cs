using NewsTella.Data;
using NewsTella.Services;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Policy;

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

		//public async Task<IActionResult> Delete(string articleId)
		//{
		//	if (string.IsNullOrEmpty(articleId))
		//	{
		//		return BadRequest("Article ID cannot be null or empty.");
		//	}

		//	var article = await _articlesService.GetArticlesById(articleId);
		//	if (article == null)
		//	{
		//		return NotFound();
		//	}

		//	var model = new ArticleEditVM
		//	{
		//		ArticleId = article.Id,
		//		Category = article.Category,
		//		LinkText = article.LinkText,
		//		Headline = article.Headline,
		//		ContentSummary = article.ContentSummary,
		//		Content = article.Content,
		//		ImageLink = article.ImageLink
		//	};

		//	return View(model);
		//}

		//// POST: User/SoftDelete/5
		//[HttpPost, ActionName("DeleteConfirmed")]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> DeleteConfirmed(string articleId)
		//{
		//	var article = await _articlesService.GetArticlesById(articleId);
		//	if (article == null)
		//	{
		//		return NotFound();
		//	}

		//	article.IsDeleted = true;
		//	var result = await _articlesService.UpdateAsync(article);
		//	if (!result.Succeeded)
		//	{
		//		ModelState.AddModelError("", "Error marking user as deleted");
		//		return View();
		//	}

		//	return RedirectToAction(nameof(Index));
		//}

		public IActionResult Details(int id)
		{
			var article = _articlesService.GetArticlesById(id);
			return View(article);
		}
		public IActionResult Preview(int id)
		{
			var article = _articlesService.GetArticlesById(id);
			return View(article);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(int id, Article article)
		{
			article.DateStamp = DateTime.Now;

			if (id != article.Id)
			{
				return NotFound();
			}

			_articlesService.UpdateArticle(article);

			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult Status(int id)
		{
			var article = _articlesService.GetArticlesById(id);
			if (article == null)
			{
				return NotFound();
			}

			article.Status = "Published2";
			_articlesService.UpdateArticle(article);
			return RedirectToAction(nameof(Index));
		}

		//public IActionResult ArticleEdit(int id)
		//{
		//    var newArticle = _articleService.GetArticleById(id);

		//    return View(newArticle);
		//}

		//[HttpPost]
		//public async Task<IActionResult> ArticleEdit(int id, Article article)
		//{
		//    article.DateStamp = DateTime.Now;

		//    if (id != article.Id)
		//    {
		//        return NotFound();
		//    }

		//    _articleService.UpdateArticle(article);

		//    return RedirectToAction("Index");
		//}

		//[HttpPost]
		//public IActionResult Submit(int id)
		//{
		//    var article = _articleService.GetArticleById(id);
		//    if (article == null)
		//    {
		//        return NotFound();
		//    }

		//    article.Status = "Submitted";
		//    _articleService.SubmitArticle(article);
		//    return RedirectToAction(nameof(Index));
		//}


	}
}
