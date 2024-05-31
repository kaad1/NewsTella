//using NewsTella.Migrations;
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
    
        public List<Article> GetAllArticles();
        public List<Article> GetArticles();    
    
        public ICollection<Article> FindByCategory(string category);
   

		//Task UpdateAsync(string article);
    
		//Task DeleteConfirmed(string articleId);
	}

}
