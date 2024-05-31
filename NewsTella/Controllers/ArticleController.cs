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
		private readonly ICategoryService _categoryService;

		public ArticleController(IArticlesService articlesService, ICategoryService categoryService)
		{
			_articlesService = articlesService;
			_categoryService = categoryService;
		}
        public IActionResult Index()
        {
            return View(_articlesService.GetAllArticles());
        }
       

        public IActionResult Create()
        { 

            return View();
        }

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

        [HttpPost]
        public async Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                article.Category = string.Join(", ", article.Cathegories);

                if (article.FormImage != null && article.FormImage.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");
                    var fileName = Path.GetFileName(article.FormImage.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await article.FormImage.CopyToAsync(fileStream);
                    }
                    article.ImageLink = "/Images/" + fileName;
                }

                _articlesService.AddArticle(article);
                return RedirectToAction("Index");
            }
            return View(article);
        }



        //[HttpPost]
        //public async Task<IActionResult> Create(Article article)
        //{
        //    article.Category = string.Join(", ", article.Cathegories);

        //   // var file = article.FormImage;
        //    //if (file != null && file.Length > 0)
        //    //{
        //    //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images\", file.FileName);
        //    //    article.ImageLink = "/Images/" + file.FileName;
        //    //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    //    {
        //    //        await file.CopyToAsync(stream);
        //    //    }
        //    //}

        //    _articlesService.AddArticle(article);
        //    return RedirectToAction("Index");


        //}


        public IActionResult Edit(int Id)
        {
            var article = _articlesService.GetArticleById(Id);
            return View(article);
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
            _articlesService.DeleteArticle(article);
            return RedirectToAction("Index");
        }

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

        public IActionResult Details(int id)
        {
            var article = _articlesService.GetArticleById(id);
            return View(article);
        }
     
               
        public IActionResult ChangeStatus(int id, string status)
        {
            var article =_articlesService.GetArticleById(id);
            if (article != null)
            {
                article.Status = status;
                _articlesService.UpdateArticleStatus(article);
            }
            // return View(article);
            return RedirectToAction("Index");
        }
        
        
        _______
        public IActionResult Index()
		{
			var model = _articlesService.GetArticles();
			return View(model);
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
			if (ModelState.IsValid)
			{

				var article = new Article
				{
					LinkText = model.LinkText,
					Headline = model.Headline,
					ContentSummary = model.ContentSummary,
					Content = model.Content,
					FormImage = model.FormImage,
					Categories = _categoryService.GetCategories().Where(c => model.SelectedCategoryIds.Contains(c.Id)).ToList()
				};
				_articlesService.AddArticle(article);
				return RedirectToAction("Index");
			}
			model.AllCategories = _categoryService.GetCategories();
			return View(model);
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
        //public IActionResult Delete(int id)
        //{
        //	var article = _articlesService.GetArticlesById(id);
        //	return View(article);
        //}
        //[HttpPost]
        //public IActionResult Delete(Article article)
        //{
        //	_articlesService.DeleteArticle(article);
        //	return RedirectToAction("Index");
        //}

  //      public async Task<IActionResult> Delete(string userId)
  //      {
  //          if (string.IsNullOrEmpty(userId))
  //          {
  //              return BadRequest("User ID cannot be null or empty.");
  //          }

  //          var user = await _articlesService.GetArticlesById(id);
  //          if (user == null)
  //          {
  //              return NotFound();
  //          }

  //          var model = new ArticleEditVM
  //          {
  //              ArticleId = article.Id,
  //              LinkText = user.LinkText,
  //              Headline = user.Headline,
  //              Email = user.Email,
  //          };

  //          return View(model);
  //      }

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
		//}
	}


    }


}
