using NewsTella.Models.Database;

namespace NewsTella.Models.ViewModel
{
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalArticles { get; set; }
        public int TotalSubscriptions { get; set; }
        public List<Article> RecentArticles { get; set; }
    }
}
