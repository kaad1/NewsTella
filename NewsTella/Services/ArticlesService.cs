using NewsTella.Models.ViewModel;
using NewsTella.Models.Database;
using NewsTella.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;

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

        public List<LatestArticleVM> GetLatestArticles(int articleCount)
        {
            return _db.Articles
                      .Include(a => a.Categories)
                      .OrderByDescending(a => a.DateStamp)
                      .Take(articleCount)
                      .Select(a => new LatestArticleVM
                      {
                          ArticleId = a.Id,
                          ImageLink = a.ImageLink,
                          Headline = a.Headline,
                          CategoryIds = a.Categories.Select(c => c.Id).ToList(),
                          CategoryNames = a.Categories.Select(c => c.Name).ToList()
                      })
                      .ToList();
        }

		public Article GetLatestArticle()
		{
            var article =  _db.Articles.Where(a => !a.IsDeleted && a.Status == "Published").OrderByDescending(a => a.DateStamp).FirstOrDefault();

			return article;
		}

    }
}
