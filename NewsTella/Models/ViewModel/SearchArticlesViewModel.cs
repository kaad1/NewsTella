using NewsTella.Models.Database;
using X.PagedList;

namespace NewsTella.Models.ViewModel
{
	public class SearchArticlesViewModel
	{
		public string SearchValue { get; set; }
        public IPagedList<Article> Articles { get; set; }
    }
}
