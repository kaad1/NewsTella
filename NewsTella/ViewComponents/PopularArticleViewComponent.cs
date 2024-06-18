using Microsoft.AspNetCore.Mvc;
using NewsTella.Services;

namespace NewsTella.ViewComponents
{
    public class PopularArticleViewComponent : ViewComponent
    {
        private readonly IArticlesService _articlesService;

        public PopularArticleViewComponent(IArticlesService articlesService)
        {
            _articlesService = articlesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            var articles = _articlesService.GetMostPopularArticles(count);
            return View(articles);
        }
    }
}
