using NewsTella.Models.Database;

namespace NewsTella.Models.ViewModel
{
	public class SearchArticlesViewModel
	{
		public string SearchValue { get; set; }
		public ICollection<Article> Articles { get; set; }
	}
}
