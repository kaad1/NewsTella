﻿namespace NewsTella.Models.ViewModel
{
    public class LatestArticleVM
    {
        public int ArticleId { get; set; }
        public string ImageLink { get; set; }
        public List<string> CategoryNames { get; set; } = new List<string>();
        public List<int> CategoryIds { get; set; } = new List<int>();
        public string Headline { get; set; }
    }
}