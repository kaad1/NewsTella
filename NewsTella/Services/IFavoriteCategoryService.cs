using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;

namespace NewsTella.Services
{
    public interface IFavoriteCategoryService
    {
        IEnumerable<FavoriteCategory> GetFavoriteCategoriesByUser(string userId);

        void AddCategories(IEnumerable<FavoriteCategory> favoriteCategories);

        void RemoveCategories(IEnumerable<FavoriteCategory> favoriteCategories);

        public List<int> GetFavoriteCategoryIdsByUser(string userId);

    }
}
