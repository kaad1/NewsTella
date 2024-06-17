using Microsoft.AspNetCore.Mvc;
using NewsTella.Services;

namespace NewsTella.ViewComponents
{
    public class EditorsChoiceArticleViewComponent: ViewComponent
    {
        private readonly IArticlesService _articlesService;

        public EditorsChoiceArticleViewComponent(IArticlesService articlesService)
        {
            _articlesService = articlesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }

    }
}
