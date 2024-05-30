using NewsTella.Models.ViewModel;
using NewsTella.Models.Database;
using NewsTella.Data;
using Microsoft.AspNetCore.Mvc;
//using NewsTella.Migrations;

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
        public Article GetArticleById(int id)
        {
            var article = _db.Articles.FirstOrDefault(a => a.Id == id);
            return article;
        }
        public List<Article> GetAllArticles()
        {
            var article = _db.Articles.ToList();
            return article;
        }
              
        public void UpdateArticle(Article article)
        {
            article.DateStamp = DateTime.Now;
            article.DateStamp = DateTime.Now;
            article.Status = "Published";            

            article.ImageLink = "https://dummyimage.com/600x400/000/fff";

            _db.Update(article);
            _db.SaveChanges();
        }
        





    }
}
