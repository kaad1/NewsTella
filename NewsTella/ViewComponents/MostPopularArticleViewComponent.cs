using Microsoft.AspNetCore.Mvc;
using NewsTella.Services;

namespace NewsTella.ViewComponents
{
    public class MostPopularArticleViewComponent : ViewComponent
    {
        private readonly IArticlesService _articlesService;

        public MostPopularArticleViewComponent(IArticlesService articlesService)
        {
            _articlesService = articlesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
