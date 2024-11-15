﻿using NewsTella.Services;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NewsTella.Data;
using System;
using Microsoft.AspNetCore.Identity;
using X.PagedList.Extensions;

namespace NewsTella.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticlesService _articlesService;
        private readonly ICategoryService _categoryService;
        private readonly IFavoriteCategoryService _favoriteCategoryService;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;


        public ArticleController(IArticlesService articlesService,
                                 ICategoryService categoryService,
                                 IFavoriteCategoryService favoriteCategoryService,
                                 UserManager<User> userManager,
                                 AppDbContext context)
        {
            _articlesService = articlesService;
            _categoryService = categoryService;
            _favoriteCategoryService = favoriteCategoryService;
            _userManager = userManager;
            _context = context;
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

            int pageSize = 5; // Number of articles per page
            int pageNumber = (page ?? 1); // Default to first page

            var pagedArticles = articles.ToPagedList(pageNumber, pageSize);
            return View(pagedArticles); // Return IPagedList<Article>
        }

        [HttpGet]
        public async Task<IActionResult> MyArticle(string headline, int? page)
        {
            ICollection<Article> articles = new List<Article>();

            if (!string.IsNullOrEmpty(headline))
            {
                articles = _articlesService.FindByHeadline(headline);
            }
            else
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                articles = _articlesService.GetArticlesByUserId(userId);
            }

            int pageSize = 5; // Number of articles per page
            int pageNumber = (page ?? 1); // Default to first page

            var pagedArticles = articles.ToPagedList(pageNumber, pageSize);
            return View(pagedArticles); // Return IPagedList<Article>
        }

        //[HttpGet]
        //public async Task<IActionResult> EditorsChoice()
        //{
        //    var latestArticle = _articlesService.GetArticles();
        //    return View(latestArticle);
        //    //ICollection<Article> articles = new List<Article>();
        //    //articles = _articlesService.GetLatestArticles();
        //    //return View(articles);
        //}

        [HttpGet]
        public async Task<IActionResult> EditorsChoice()
        {
            var articles = _articlesService.GetArticlesForEditorsChoice();
            return View(articles);
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
            ModelState.Remove("FormImage");
            ModelState.Remove("SelectedCategoryIds");
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var article = new Article
                {
                    LinkText = model.LinkText,
                    Headline = model.Headline,
                    ContentSummary = model.ContentSummary,
                    Content = model.Content,
                    FormImage = model.FormImage,
                    User = user,
                };

                article.Categories = _categoryService.GetCategories().Where(c => model.SelectedCategoryIds.Contains(c.Id)).ToList();


                //if (model.FormImage != null && model.FormImage.Length > 0)
                //{
                //    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");
                //    var fileName = Path.GetFileName(model.FormImage.FileName);
                //    var filePath = Path.Combine(uploadFolder, fileName);
                //    using (var fileStream = new FileStream(filePath, FileMode.Create))
                //    {
                //        await model.FormImage.CopyToAsync(fileStream);
                //    }
                //    article.ImageLink = "/Images/" + fileName;
                //}

                _articlesService.AddArticle(article);
                return RedirectToAction("Index");
            }
            else
            {
                model.AllCategories = _categoryService.GetCategories();
                return View(model);
            }
        }

        public IActionResult Edit(int Id)
        {
            var article = _articlesService.GetArticleById(Id);
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
        public async Task<IActionResult> Edit(ArticleEditVM model)
        {
            ModelState.Remove("AllCategories");
            ModelState.Remove("ImageLink");
            ModelState.Remove("FormImage");
            ModelState.Remove("SelectedCategoryIds");
            if (ModelState.IsValid)
            {
                Article article = _articlesService.GetArticleById(model.Id);
                article.LinkText = model.LinkText;
                article.Headline = model.Headline;
                article.ContentSummary = model.ContentSummary;
                article.Content = model.Content;
                if (model.SelectedCategoryIds != null)
                {
                    article.Categories = _categoryService.GetCategories().Where(c => model.SelectedCategoryIds.Contains(c.Id)).ToList();
                }
                else
                {
                    article.Categories = new List<Category>();
                }
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

                _articlesService.UpdateArticle(article);
                return RedirectToAction("Index");
            }
            else
            {
                model.AllCategories = _categoryService.GetCategories();
                return View(model);
            }
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


        public IActionResult ChangeStatus(int id, string status)
        {
            var article = _articlesService.GetArticleById(id);
            if (article != null)
            {
                if (User.IsInRole("Editor"))
                {
                    _articlesService.UpdateArticleStatusPublished(article);
                }
                else if (User.IsInRole("Writer"))
                {
                    _articlesService.UpdateArticleStatusSubmitted(article);
                }
                _articlesService.UpdateArticleStatus(article);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Like([FromBody] int id)
        {
            try
            {
                await _articlesService.LikeArticleAsync(id);
                Article article = _articlesService.GetArticleById(id);
                int likeCount = article.Likes;
                return Ok(new { success = true, likeCount = likeCount, message = "Article liked successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var article = _articlesService.GetArticleById(id);
            if (article == null)
            {
                return NotFound();
            }
            else if (article.Status == "Published")
            {
                await _articlesService.IncrementViewsAsync(id);
                return View(article);
            }

            return View(article);
        }
        //         if (article == null)
        //            {
        //                return NotFound();
        //    }
        //            else if(article.Status == "Published")
        //            {
        //                await _articlesService.IncrementViewsAsync(id);
        //                return View(article);
        //}  

        //[HttpGet]
        //public async Task<IActionResult> SearchArticles(string headline, int? page)
        //{
        //    ICollection<Article> articles = new List<Article>();

        //    if (!string.IsNullOrEmpty(headline))
        //    {
        //        articles = _articlesService.FindByHeadline(headline);
        //    }
        //    else
        //    {
        //        articles = _articlesService.GetArticles();
        //    }

        //    var publishedArticles = articles.Where(a => a.Status == "Published").ToList();

        //    int pageSize = 12; // Number of articles per page
        //    int pageNumber = (page ?? 1); // Default to first page

        //    var pagedArticles = publishedArticles.ToPagedList(pageNumber, pageSize);

        //    var searchArticles = new SearchArticlesViewModel
        //    {
        //        SearchValue = headline,
        //        Articles = pagedArticles
        //    };

        //    return View(searchArticles);
        //}

         [HttpGet]
        public async Task<IActionResult> SearchArticles(string headline, int? page)
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

			var publishedArticles = articles.Where(a => a.Status == "Published").ToList();
            
            int pageSize = 12; // Number of articles per page
			int pageNumber = (page ?? 1); // Default to first page

			var pagedArticles = publishedArticles.ToPagedList(pageNumber, pageSize);
			
            var searchArticles = new SearchArticlesViewModel
			{
				SearchValue = headline,
				Articles = pagedArticles
			};

			return View(searchArticles);
        }

        [HttpPost]
        public IActionResult UpdateEditorsChoice(int[] selectedEditorsChoiceIds)
        {
            const int maxSelections = 6;
            ICollection<Article> articles = _articlesService.GetArticles();
            if (selectedEditorsChoiceIds.Length > maxSelections)
            {
                ModelState.AddModelError(string.Empty, $"You can only select up to {maxSelections} articles");
                return View("EditorsChoice", articles);
            }

            foreach (var article in articles)
            {
                _articlesService.UpdateEditorsChoiceStatus(article.Id, selectedEditorsChoiceIds.Contains(article.Id));
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult LatestArticleByFavoriteCategory()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var favoriteCategoryIds = _favoriteCategoryService.GetFavoriteCategoryIdsByUser(userId);
            var latestArticle = _articlesService.GetLatestArticleByCategoryIds(favoriteCategoryIds);

            if (latestArticle == null)
            {
                return View("NoArticles"); // Create a view to handle no articles found case
            }

            return View(latestArticle);
        }

        public IActionResult SearchArchivedNews(string Headline)
        {
            var results = _articlesService.GetArchivedNewsByHeadLine(Headline);

            return View(results);
        }

    }
}
