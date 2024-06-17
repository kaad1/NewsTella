using NewsTella.Models.Database;

namespace NewsTella.Models.ViewModel
{
    public class FavoriteCategoriesVM
    {
        public List<Category> AllCategories { get; set; } = new List<Category>();
        public List<int> SelectedCategoryIds { get; set; } = new List<int>();

    }
}
