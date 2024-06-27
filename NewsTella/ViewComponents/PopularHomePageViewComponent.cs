using Microsoft.AspNetCore.Mvc;
using NewsTella.Services;

namespace NewsTella.ViewComponents
{
    public class PopularHomePageViewComponent : ViewComponent
    {
        private readonly IArticlesService _articlesService;

        public PopularHomePageViewComponent(IArticlesService articlesService)
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
