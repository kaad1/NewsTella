using NewsTella.Models.Database;

namespace NewsTella.Services
{
	public interface ICategoryService
	{
		List<Category> GetCategories();

		Category GetCategoryById(int id);

		void AddCategory(Category category);

		void RemoveCategory(Category category);

		void UpdateCategory(Category category);
	}
}
