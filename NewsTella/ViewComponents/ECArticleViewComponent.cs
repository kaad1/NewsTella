using Microsoft.AspNetCore.Mvc;
using NewsTella.Services;

namespace NewsTella.ViewComponents
{
    public class ECArticleViewComponent: ViewComponent
    {
        private readonly IArticlesService _articlesService;

        public ECArticleViewComponent(IArticlesService articlesService)
        {
            _articlesService = articlesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            var articles = _articlesService.GetEditorsChoiceArticles(count);
            return View(articles);
        }

    }
}
