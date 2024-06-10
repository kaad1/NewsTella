using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.Database;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsTella.Models.ViewModel
{
    public class ArticleVM
    {
        public int ArticleId { get; set; }
        public string LinkText { get; set; }
        public string Headline { get; set; }
        public string ContentSummary { get; set; }
        public string Content { get; set; }
        public string ImageLink { get; set; }
        public string Category { get; set; }
        public string Status { get; set; } = "Draft";
        public List<Article> ArticleList { get; set; } = new List<Article>();
        public List<Article> LatestArticles { get; set; }
        public List<Category> AllCategories { get; set; }

    }
}