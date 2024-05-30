using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Models;
using NewsTella.Models.ViewModel;
using NewsTella.Services;
using System.Diagnostics;

namespace NewsTella.Controllers
{
	//[Authorize(Roles = "Admin")]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IArticlesService _articlesService;

		public HomeController(ILogger<HomeController> logger, IArticlesService articlesService)
		{
			_logger = logger;
			_articlesService = articlesService;
		}

		public IActionResult Index()
		{
			ArticleVM model = new ArticleVM();
			model.ArticleList = _articlesService.GetArticles();
			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		
	}
}
