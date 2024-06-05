using NewsTella.Migrations;
using NewsTella.Models.Database;

namespace NewsTella.Services
{
	public interface IArticlesService
	{
		public void AddArticle(Article article);
    
		public void UpdateArticle(Article article);    

        public void UpdateArticleStatus(Article article);
    
        public void DeleteArticle(Article article);
        public Article GetArticleById(int id);
             
        public List<Article> GetArticles();
            
        public ICollection<Article> FindByCategory(string category);

        public ICollection<Article> FindByHeadline(string headline);
        public Task LikeArticleAsync(int id);
              
        public Task IncrementViewsAsync(int id);
        public Task GetArticleByIdAsync(int id, Article article);
        public Task GetArticleByIdAsync();
        Task<string?> GetArticleByIdAsync(int id);
    }

}
