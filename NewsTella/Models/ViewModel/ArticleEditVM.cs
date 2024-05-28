using NewsTella.Models.Database;

namespace NewsTella.Models.ViewModel
{
	public class ArticleEditVM
	{		
		public int ArticleId { get; set; }
		public string LinkText { get; set; }
		public string Headline { get; set; }
		public string ContentSummary { get; set; }
		public string Category { get; set; }
		public string Content { get; set; }
		public string ImageLink { get; set; }
		public List<string> Cathegories { get; set; }
		public IList <string> SelectedCathegories { get; set; }		
	}
}
