using Microsoft.AspNetCore.Mvc;
using NewsTella.Services;

namespace NewsTella.ViewComponents
{
    public class LatestArticleViewComponent : ViewComponent
    {
        private readonly IArticlesService _articlesService;

        public LatestArticleViewComponent(IArticlesService articlesService)
        {
            _articlesService = articlesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            var latestArticle = _articlesService.GetLatestArticles(count);
            return View(latestArticle);
        }
    }
}