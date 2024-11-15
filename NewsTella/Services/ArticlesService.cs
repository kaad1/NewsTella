﻿using NewsTella.Models.ViewModel;
using NewsTella.Models.Database;
using NewsTella.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;

namespace NewsTella.Services
{
	public class ArticlesService : IArticlesService
	{
		private readonly AppDbContext _db;
		private readonly IConfiguration _configuration;

		public ArticlesService(AppDbContext db, IConfiguration configuration)
		{
			_db = db;
			_configuration = configuration;
		}

        public void AddArticle(Article article)
		{
			UploadFilesToContainer(article);

            _db.Articles.Add(article);
			_db.SaveChanges();
		}
		public void UpdateArticle(Article article)
		{
			article.DateStamp = DateTime.Now;
			article.Status = "Edited";
			
			_db.Articles.Update(article);
			_db.SaveChanges();
		}
		public void DeleteArticle(Article article)
		{
			_db.Articles.Remove(article);
			_db.SaveChanges();
		}

		public Article GetArticleById(int id)
		{
			var article = _db.Articles.Include(a => a.Categories).FirstOrDefault(a => a.Id == id);
			return article;
		}
		public List<Article> GetArticles()
		{
			var article = _db.Articles.Where(a => a.IsDeleted == false).Include(a => a.Categories).ToList();
			return article;
		}

		public void UpdateArticleStatus(Article article)
		{
			article.DateStamp = DateTime.Now;
			article.Status = article.Status;

			_db.Update(article);
			_db.SaveChanges();
		}
		public void UpdateArticleStatusPublished(Article article)
		{
			article.DateStamp = DateTime.Now;
			article.Status = "Published";

			_db.Update(article);
			_db.SaveChanges();

		}
		public void UpdateArticleStatusSubmitted(Article article)
		{
			article.DateStamp = DateTime.Now;
			article.Status = "Submitted";

			_db.Update(article);
			_db.SaveChanges();
		}


		public ICollection<Article> FindByCategory(string category)
		{
			return _db.Categories.Where(c => c.Name == category).FirstOrDefault().Articles.Where(a => a.IsDeleted == false).ToList();
		}

		public ICollection<Article> FindByHeadline(string headline)
		{
			return _db.Articles.Where(a => a.Headline.Contains(headline) && a.IsDeleted == false).ToList();
		}
		public async Task LikeArticleAsync(int id)
		{
			var article = await _db.Articles.FindAsync(id);
			if (article == null)
			{
				throw new Exception("Article not found");
			}
			else if (article.Status == "Published")
			{
				article.Likes += 1;
				_db.Update(article);
				await _db.SaveChangesAsync();
			}
		}
		public async Task IncrementViewsAsync(int id)
		{
			var article = await _db.Articles.FindAsync(id);
			if (article == null)
			{
				throw new Exception("Article not found");
			}

			article.Views += 1;
			_db.Update(article);
			await _db.SaveChangesAsync();
		}

		public Task GetArticleByIdAsync(int id, Article article)
		{
			throw new NotImplementedException();
		}
		public List<Article> LatestArticles(int articleCount)
        {
            var latestArticles = _db.Articles.Include(a => a.Categories).OrderByDescending(a => a.DateStamp).Take(articleCount).ToList();
            return latestArticles;
        }

		public Task GetArticleByIdAsync()
		{
			throw new NotImplementedException();
		}

		public Task<string?> GetArticleByIdAsync(int id)
		{
			throw new NotImplementedException();
		}


        public Article UploadFilesToContainer(Article article)
        {
            BlobContainerClient blobServiceClient = new BlobServiceClient(
                                   _configuration["AzureWebJobsStorage"]).GetBlobContainerClient("newstellacontainer");
            //foreach (var file in article.FormImage)
            //{
                BlobClient blobClient = blobServiceClient.GetBlobClient(article.FormImage.FileName);
                using (var stream = article.FormImage.OpenReadStream())
                {
                    blobClient.Upload(stream);
                }
                article.ImageLink = blobClient.Uri.AbsoluteUri;
            //}
            return article;

        }

        public List<FrontPageArticleVM> GetLatestArticles(int articleCount)
        {
            var articles = _db.Articles
                      .Include(a => a.Categories)
					  .Where(a => !a.IsDeleted && a.Status == "Published")
					  .OrderByDescending(a => a.DateStamp)
                      .Take(articleCount)
                      .Select(a => new FrontPageArticleVM
                      {
                          ArticleId = a.Id,
                          ImageLink = a.ImageLink,
                          Headline = a.Headline,
                          CategoryIds = a.Categories.Select(c => c.Id).ToList(),
                          CategoryNames = a.Categories.Select(c => c.Name).ToList(),
						  Views = a.Views
                      })
                      .ToList();
			return articles;
        }

		public List<FrontPageArticleVM> GetMostPopularArticles(int articleCount)
		{
			var articles = _db.Articles
				.Include(a => a.Categories)
				.Where(a => !a.IsDeleted && a.Status == "Published")
				.OrderByDescending(a => a.Views)
				.ThenByDescending(a => a.DateStamp)
				.Take(articleCount)
				.Select(a => new FrontPageArticleVM
				{
					ArticleId = a.Id,
					ImageLink = a.ImageLink,
					Headline = a.Headline,
					CategoryIds = a.Categories.Select(c => c.Id).ToList(),
					CategoryNames = a.Categories.Select (c => c.Name).ToList(),
					Views = a.Views
				})
				.ToList();
			return articles;
		}
		public List<FrontPageArticleVM> GetEditorsChoiceArticles(int articleCount)
		{
			var articles = _db.Articles
				.Include(a => a.Categories)
				.Where(a => !a.IsDeleted && a.Status == "Published" && a.IsEditorsChoice.Equals(true))
				.OrderByDescending(a => a.DateStamp)
				.Take(articleCount)
				.Select(a => new FrontPageArticleVM
				{
					ArticleId = a.Id,
					ImageLink = a.ImageLink,
					Headline = a.Headline,
					CategoryIds = a.Categories.Select(c => c.Id).ToList(),
					CategoryNames = a.Categories.Select(c => c.Name).ToList(),
					Views = a.Views
				})
				.ToList();
			return articles;
		}

		public List<Article> GetBreakingNews(int count)
		{
			return _db.Articles
						   .Where(a => !a.IsDeleted && a.Status == "Published")
						   .OrderByDescending(a => a.DateStamp)
						   .ThenByDescending(a => a.Views)
						   .Take(count)
						   .ToList();
		}


		public Article GetLatestArticle()
		{
            var article =  _db.Articles.Where(a => !a.IsDeleted && a.Status == "Published").OrderByDescending(a => a.DateStamp).FirstOrDefault();

			return article;
		}

        public void UpdateEditorsChoiceStatus(int id, bool isEditorsChoice)
        {
            var article = _db.Articles.Find(id);
            if (article != null)
            {
                article.IsEditorsChoice = isEditorsChoice;
                _db.SaveChanges();
            }
        }


        public List<Article> GetArticlesForEditorsChoice()
        {
            var articles = _db.Articles
                .Include(a => a.Categories)
                .Where(a => a.IsDeleted == false && a.Status == "Published")
                .OrderByDescending(a => a.IsEditorsChoice)  // First, order by Editor's Choice
                .ThenByDescending(a => a.DateStamp)        // Then, order by DateStamp
                .ToList();

            return articles;
        }

        public List<Article> GetLatestArticleByCategoryIds(IEnumerable<int> categoryIds)
        {
            // Step 1: Retrieve all relevant articles from the database.
            var articles = _db.Articles
                              .Include(a => a.Categories)
                              .Where(a => a.Categories.Any(c => categoryIds.Contains(c.Id)) && a.IsDeleted == false && a.Status == "Published")
                              .OrderByDescending(a => a.DateStamp)
                              .ToList();

            // Step 2: Group by category and select the top three articles from each category.
            var groupedArticles = articles
                                  .SelectMany(a => a.Categories.Select(c => new { CategoryId = c.Id, Article = a }))
                                  .GroupBy(x => x.CategoryId)
                                  .SelectMany(g => g.Take(3))
                                  .Select(x => x.Article)
                                  .Distinct()
                                  .ToList();

            return groupedArticles;
        }

        public List<Article> GetPublishedArticlesByCategoryId(int categoryId)
		{
			return _db.Articles
                      .Include(a => a.Categories)
                      .Where(a => a.Categories.Any(c => categoryId == c.Id) && a.IsDeleted == false && a.Status == "Published")
                      .OrderByDescending(a => a.DateStamp)
					  .ToList();
		}

        public List<Article> GetArchivedNewsByHeadLine(string Headline)
		{
            return _db.Articles
                .Where(a => a.Status == "Archived" && (a.Headline.Contains(Headline) && a.Content.Contains(Headline)))
                .ToList();
        }

		public List<Article> GetArticlesByUserId(string userId)
		{
			return _db.Articles
				.Where(a => a.User.Id == userId)
				.ToList();
		}
	}
}
