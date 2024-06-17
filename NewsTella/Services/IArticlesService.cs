
using NewsTella.Models.Database;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

using Microsoft.AspNetCore.Identity;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;


namespace NewsTella.Services
{
	public interface IArticlesService
	{
		public void AddArticle(Article article);    
		public void UpdateArticle(Article article);    
        public void UpdateArticleStatus(Article article);
        public void UpdateArticleStatusPublished(Article article);
        public void UpdateArticleStatusSubmitted(Article article);


		public ICollection<Article> FindByCategory(string category);

        public ICollection<Article> FindByHeadline(string headline);

      


        //Task UpdateAsync(string article);

        //Task DeleteConfirmed(string articleId);

        public void DeleteArticle(Article article);

        public Article GetArticleById(int id);             
        public List<Article> GetArticles();
        public List<FrontPageArticleVM> GetLatestArticles(int articleCount);
        public List<FrontPageArticleVM> GetMostPopularArticles(int articleCount);
        public List<FrontPageArticleVM> GetEditorsChoiceArticles(int articleCount);
        public Article GetLatestArticle();


		public Task LikeArticleAsync(int id);              
        public Task IncrementViewsAsync(int id);

        public Task GetArticleByIdAsync(int id, Article article);
        public Task GetArticleByIdAsync();
        Task<string?> GetArticleByIdAsync(int id);
        public void UpdateEditorsChoiceStatus(int id, bool isEditorsChoice);

    }

}
