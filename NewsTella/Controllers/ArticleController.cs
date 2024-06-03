using NewsTella.Data;
using NewsTella.Services;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Policy;
using X.PagedList;
using NewsTella.Migrations;

namespace NewsTella.Controllers
{
	public class ArticleController : Controller
	{
		private readonly IArticlesService _articlesService;
		private readonly ICategoryService _categoryService;

		public ArticleController(IArticlesService articlesService, ICategoryService categoryService)
		{
			_articlesService = articlesService;
			_categoryService = categoryService;
		}

        [HttpGet]
        public async Task<IActionResult> Index(string headline, int? page)
        {
            ICollection<Article> articles = new List<Article>();

            if (!string.IsNullOrEmpty(headline))
            {
                articles = _articlesService.FindByHeadline(headline);
            }
            else
            {
                articles = _articlesService.GetArticles();
            }

            int pageSize = 4; // Number of articles per page
            int pageNumber = (page ?? 1); // Default to first page

            var pagedArticles = articles.ToPagedList(pageNumber, pageSize);
            return View(pagedArticles); // Return IPagedList<Article>
        }

        public IActionResult Create()
		{
			ArticleCreateVM model = new ArticleCreateVM();
			model.AllCategories = _categoryService.GetCategories();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(ArticleCreateVM model)
		{
			ModelState.Remove("AllCategories");
			ModelState.Remove("ImageLink");
			if (ModelState.IsValid)
			{
				var article = new Article
				{
					LinkText = model.LinkText,
					Headline = model.Headline,
					ContentSummary = model.ContentSummary,
					Content = model.Content,
				};

				article.Categories = _categoryService.GetCategories().Where(c => model.SelectedCategoryIds.Contains(c.Id)).ToList();


                if (model.FormImage != null && model.FormImage.Length > 0)
				{
					var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");
					var fileName = Path.GetFileName(model.FormImage.FileName);
					var filePath = Path.Combine(uploadFolder, fileName);
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await model.FormImage.CopyToAsync(fileStream);
					}
					article.ImageLink = "/Images/" + fileName;
				}

				_articlesService.AddArticle(article);
				return RedirectToAction("Index");
			}
			else
			{
				model.AllCategories = _categoryService.GetCategories();
				return View(model);
			}
		}



		//public IActionResult Edit(int Id)
		//{
		//	var article = _articlesService.GetArticleById(Id);
		//	var model = new ArticleEditVM 
		//	{
		//		Id = article.Id,
		//		DateStamp = article.DateStamp,
		//		LinkText = article.LinkText,
		//		Headline = article.Headline,
		//		ContentSummary = article.ContentSummary,
		//		Content = article.Content,
		//		FormImage = article.FormImage,
		//		ImageLink = article.ImageLink,
		//		SelectedCategoryIds = article.Categories.Select(c => c.Id).ToList(),
		//		AllCategories = _categoryService.GetCategories()
		//	};
		//	return View(article);
		//}

        public IActionResult Edit(int id)
        {
            var article = _articlesService.GetArticleById(id);
            if (article == null)
            {
                return NotFound();
            }

            var model = new ArticleEditVM
            {
                Id = article.Id,
                DateStamp = article.DateStamp,
                LinkText = article.LinkText,
                Headline = article.Headline,
                ContentSummary = article.ContentSummary,
                Content = article.Content,
                ImageLink = article.ImageLink,
                SelectedCategoryIds = article.Categories.Select(c => c.Id).ToList(),
                AllCategories = _categoryService.GetCategories()
            };
            return View(model);
        }
        
             [HttpPost]
        public IActionResult Edit(Article article)
        {
        	_articlesService.UpdateArticle(article);
        	return RedirectToAction("Index");

        }
        public IActionResult Delete(int id)

		{
			var article = _articlesService.GetArticleById(id);
			return View(article);
		}

		[HttpPost]
		public IActionResult Delete(Article article)
		{
			article = _articlesService.GetArticleById(article.Id);
			article.IsDeleted = true;
			_articlesService.UpdateArticle(article);
			return RedirectToAction("Index");
		}			
	
		public IActionResult Details(int id)
		{
			var article = _articlesService.GetArticleById(id);
			return View(article);
		}
		public IActionResult ChangeStatus(int id, string status)
		{
			var article = _articlesService.GetArticleById(id);
			if (article != null)
			{
				article.Status = status;
				_articlesService.UpdateArticleStatus(article);
			}

			return RedirectToAction("Index");
		}





		//[HttpPost]
		//public async Task<IActionResult> Create(ArticleCreateVM model)
		//{
		//	ModelState.Remove("AllCategories");
		//	if (ModelState.IsValid)
		//	{
		//		var article = new Article
		//		{
		//			LinkText = model.LinkText,
		//			Headline = model.Headline,
		//			ContentSummary = model.ContentSummary,
		//			Content = model.Content,
		//			FormImage = model.FormImage,
		//			Categories = _categoryService.GetCategories().Where(c => model.SelectedCategoryIds.Contains(c.Id)).ToList()
		//		};
		//		_articlesService.AddArticle(article);
		//		return RedirectToAction("Index");
		//	}
		//	model.AllCategories = _categoryService.GetCategories();
		//	return View(model);
		//}

		//public async Task<IActionResult> Edit(int Id)
		//{
		//	var article = _articlesService.GetArticleById(Id);
		//	var model = new ArticleEditVM
		//	{
		//		Id = article.Id,
		//		DateStamp = article.DateStamp,
		//		LinkText = article.LinkText,
		//		Headline = article.Headline,
		//		ContentSummary = article.ContentSummary,
		//		Content = article.Content,
		//		FormImage = article.FormImage,
		//		SelectedCategoryIds = article.Categories.Select(c => article.Id).ToList(),
		//		AllCategories = _categoryService.GetCategories()
		//	};
		//	return View(model);
		//}

		//[HttpPost]
		//public async Task<IActionResult> Edit(ArticleEditVM model)
		//{
		//	var article = _articlesService.GetArticleById(model.Id);
		//	article.LinkText = model.LinkText;
		//	article.Headline = model.Headline;
		//	article.ContentSummary = model.ContentSummary;
		//	article.Content = model.Content;
		//	article.FormImage = model.FormImage;
		//	//article.ImageLink = model.ImageLink;
		//	List<Category> allCategories = _categoryService.GetCategories();
		//	article.Categories = allCategories.Where(c => model.SelectedCategoryIds.Contains(c.Id)).ToList();
		//	_articlesService.UpdateArticle(article);
		//	return RedirectToAction("Index");
		//}


		//public IActionResult ChangeStatus(int id, string status)
		//{
		//	var article = _articlesService.GetArticleById(id);
		//	if (article != null)
		//	{
		//		article.Status = status;
		//		_articlesService.UpdateArticleStatus(article);
		//	}
		//	// return View(article);
		//	return RedirectToAction("Index");
		//}

		//[HttpPost]
		//public async Task<IActionResult> Create(Article article)
		//{
		//    article.Category = string.Join(", ", article.Cathegories);

		//    var file = article.FormImage;
		//    if (file != null && file.Length > 0)
		//    {
		//        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images\", file.FileName);
		//        article.ImageLink = "/Images/" + file.FileName;
		//        using (var stream = new FileStream(filePath, FileMode.Create))
		//        {
		//            await file.CopyToAsync(stream);
		//        }
		//    }
		//    _articlesService.AddArticle(article);
		//    return RedirectToAction("Index");

		//}
				///// Create with Uploading images 
		//[HttpPost]
		//      public async Task<IActionResult> Create(Article article)
		//      {
		//          if (ModelState.IsValid)
		//          {
		//              article.Category = string.Join(", ", article.Cathegories);

		//              if (article.FormImage != null && article.FormImage.Length > 0)
		//              {
		//                  var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");
		//                  var fileName = Path.GetFileName(article.FormImage.FileName);
		//                  var filePath = Path.Combine(uploads, fileName);
		//                  using (var fileStream = new FileStream(filePath, FileMode.Create))
		//                  {
		//                      await article.FormImage.CopyToAsync(fileStream);
		//                  }
		//                  article.ImageLink = "/Images/" + fileName;
		//              }

		//              _articlesService.AddArticle(article);
		//              return RedirectToAction("Index");
		//          }
		//          return View(article);
		//      }




		//public async Task<IActionResult> Delete(string articleId)
		//{
		//	if (string.IsNullOrEmpty(articleId))
		//	{
		//		return BadRequest("Article ID cannot be null or empty.");
		//	}

		//	var article = await _articlesService.GetArticleById(articleId);
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
		//	var article = await _articlesService.GetArticleById(articleId);
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
		

		//      public IActionResult Index()
		//{
		//	var model = _articlesService.GetArticles();
		//	return View(model);
		//}

		//[HttpPost]
		//public async Task<IActionResult> Create(Article article)
		//{

		//	//if (!ModelState.IsValid)
		//	//{ Clear Form
		//	//	ModelState.Clear(); // Rensa felmeddelanden
		//	//	return View("Index", model);
		//	//}


		//public IActionResult Edit(int Id)
		//{
		//	var article = _articlesService.GetArticlesById(Id);
		//	return View(article);
		//}
		//[HttpPost]
		//public IActionResult Edit(Article article)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		_articlesService.UpdateArticle(article);
		//		return RedirectToAction("Index");
		//	}
		//	return View(article);
		//}
		
		//      // POST: User/SoftDelete/5
		//      [HttpPost, ActionName("DeleteConfirmed")]
		//      [ValidateAntiForgeryToken]
		//      public async Task<IActionResult> DeleteConfirmed(string userId)
		//      {
		//          var user = await _articlesService.FindByIdAsync(userId);
		//          if (user == null)
		//          {
		//              return NotFound();
		//          }

		//          user.IsDeleted = true;
		//          var result = await _articlesService.UpdateArticle(article);
		//          if (!result.Succeeded)
		//          {
		//              ModelState.AddModelError("", "Error marking user as deleted");
		//              return View();
		//          }

		//          return RedirectToAction(nameof(Index));
		//      }

		//      [HttpPost]
		//      public IActionResult DeleteDraft(Article article)
		//      {
		//	ModelState.Clear(); // Rensa felmeddelanden
		//						//	return View("Index", model);
		//	return RedirectToAction("Create");
		//      }
		//      public IActionResult Details(int id)
		//{
		//	var article = _articlesService.GetArticlesById(id);
		//	return View(article);
		//	//}

		//public IActionResult Create()
		//{
		//	return View();
		//}

		//public IActionResult Edit(int Id)
		//{
		//	var article = _articlesService.GetArticleById(Id);
		//	return View(article);
		//}
		//[HttpPost]
		//public IActionResult Edit(Article article)
		//{
		//	_articlesService.UpdateArticle(article);
		//	return RedirectToAction("Index");
		//}

	}
}