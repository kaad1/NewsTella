using Microsoft.AspNetCore.Mvc;
using NewsTella.Services;

namespace NewsTella.ViewComponents
{
	public class DetailsLatestViewComponent : ViewComponent
	{
		private readonly IArticlesService _articlesService;

		public DetailsLatestViewComponent(IArticlesService articlesService)
		{
			_articlesService = articlesService;
		}

		public async Task<IViewComponentResult> InvokeAsync(int count)
		{
			var latestArticles = _articlesService.GetLatestArticles(count);
			return View(latestArticles);
		}
	}
}