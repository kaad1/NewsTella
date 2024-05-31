using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NewsTella.Models.Database;

namespace NewsTella.Models.ViewModel
{
	public class ArticleCreateVM
	{
		public string LinkText { get; set; } 

		public string Headline { get; set; } 

		public string ContentSummary { get; set; } 

		public string Content { get; set; } 

		public IFormFile FormImage { get; set; }

		public List<int> SelectedCategoryIds { get; set; }

		public List<Category> AllCategories { get; set; } 

	}
}
