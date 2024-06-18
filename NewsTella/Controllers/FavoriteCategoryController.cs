using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;
using NewsTella.Services;

namespace NewsTella.Controllers
{
    public class FavoriteCategoryController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ICategoryService _categoryService;
        private readonly IFavoriteCategoryService _favoriteCategoryService;

        public FavoriteCategoryController(UserManager<User> userManager,  ICategoryService categoryService, IFavoriteCategoryService favoriteCategoryService)
        {
            _userManager = userManager;
            _categoryService = categoryService;
            _favoriteCategoryService = favoriteCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new FavoriteCategoriesVM();
            model.AllCategories = _categoryService.GetCategories();
            model.SelectedCategoryIds = _favoriteCategoryService.GetFavoriteCategoriesByUser(user.Id).Select(c => c.CategoryId).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FavoriteCategoriesVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            IEnumerable<FavoriteCategory> oldFavoriteCategories = _favoriteCategoryService.GetFavoriteCategoriesByUser(user.Id);
            _favoriteCategoryService.RemoveCategories(oldFavoriteCategories);

            IEnumerable<FavoriteCategory> newFavoriteCategories = model.SelectedCategoryIds.Select(c => new FavoriteCategory {UserId = user.Id, CategoryId = c});
            _favoriteCategoryService.AddCategories(newFavoriteCategories);
            
            return RedirectToAction("Index", "Home");
        }
    }
}
