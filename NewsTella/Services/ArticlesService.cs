using NewsTella.Models.ViewModel;
using NewsTella.Models.Database;
using NewsTella.Data;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Migrations;

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
			
		public void DeleteArticle(Article article)
		{
			_db.Articles.Remove(article);
			_db.SaveChanges();
		}
		public Article GetArticlesById(int id)
		{
			var article = _db.Articles.FirstOrDefault(a => a.Id == id);
			return article;
		}
		public List<Article> GetArticles()
		{
			var article = _db.Articles.ToList();
			return article;
		}
		public Article Preview(int id)
		{
			var article = _db.Articles.FirstOrDefault(a => a.Id == id);
			return article;
		}
        public void UpdateArticle(Article article)
        {
            article.Status = "Draft";
            _db.Articles.Update(article);
            _db.SaveChanges();
        }      
		
        //public void UpdateArticle(Article article)

        //{

        //    article.DateStamp = DateTime.Now;

        //    article.DateStamp = DateTime.Now;

        //    string userFirstName = _httpContextAccessor.HttpContext.Session.GetString("UserFirstName");

        //    string userLastName = _httpContextAccessor.HttpContext.Session.GetString("UserLastName");

        //    article.Status = "Draft";

        //    article.UserName = userFirstName + " " + userLastName;

        //    article.ImageLink = "https://dummyimage.com/600x400/000/fff";

        //    _db.Update(article);

        //    _db.SaveChanges();

        //}       




        //public Task UpdateAsync(string article)
        //{
        //	return Task.CompletedTask;
        //}
        //Task DeleteConfirmed(string articleId)
        //{
        //	return Task.CompletedTask;
        //}









    }
}
