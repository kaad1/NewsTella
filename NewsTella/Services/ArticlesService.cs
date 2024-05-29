using NewsTella.Models.ViewModel;
using NewsTella.Models.Database;
using NewsTella.Data;

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
			_db.Articles.Update(article);
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

	}
}
