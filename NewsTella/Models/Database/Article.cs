﻿namespace NewsTella.Models.Database
{
    public class Article
    {
        public int Id { get; set; }
        public DateTime DateStamp { get; set; } = DateTime.Now;
		public string LinkText { get; set; }
        public string Headline { get; set; }
        public string ContentSummary { get; set; }
        public string Content { get; set; }
        public int Views { get; set; }
        public int Likes { get; set; }
        public string ImageLink { get; set; }
        public string Category { get; set; }
       // public string Status { get; set; }

    }
}
