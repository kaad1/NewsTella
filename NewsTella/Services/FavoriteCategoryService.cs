using Microsoft.EntityFrameworkCore;
using NewsTella.Data;
using NewsTella.Models.Database;

namespace NewsTella.Services
{
    public class FavoriteCategoryService : IFavoriteCategoryService
    {
        private readonly AppDbContext _db;

        public FavoriteCategoryService(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<FavoriteCategory> GetFavoriteCategoriesByUser(string userId)
        {
            List<FavoriteCategory> favoriteCategories = _db.FavoriteCategories.Where(c => c.UserId == userId).ToList();
            return favoriteCategories;
        }

        public void AddCategories(IEnumerable<FavoriteCategory> favoriteCategories)
        {
            _db.FavoriteCategories.AddRange(favoriteCategories);
            _db.SaveChanges();
        }

        public void RemoveCategories(IEnumerable<FavoriteCategory> favoriteCategories)
        {
            _db.FavoriteCategories.RemoveRange(favoriteCategories);
            _db.SaveChanges();
        }
    }
}
