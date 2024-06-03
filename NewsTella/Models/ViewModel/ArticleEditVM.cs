using NewsTella.Models.Database;

namespace NewsTella.Models.ViewModel
{
    public class ArticleEditVM
    {
        public int Id { get; set; }
        public DateTime DateStamp { get; set; }
        public string LinkText { get; set; }
        public string Headline { get; set; }
        public string ContentSummary { get; set; }
        public string Content { get; set; }
        public IFormFile FormImage { get; set; }
        public string ImageLink { get; set; }
        public List<int> SelectedCategoryIds { get; set; }
        public List<Category> AllCategories { get; set; }
    }
}

