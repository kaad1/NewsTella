using NewsTella.Models.Database;
namespace NewsTella.Services
{
	public interface IArticlesService
	{
		public void AddArticle(Article article);
		public void UpdateArticle(Article article);
		public void DeleteArticle(Article article);
		public Article GetArticlesById(int id);
		public List<Article> GetArticles();
		public ICollection<Article> FindByCategory(string category);
    }
}
