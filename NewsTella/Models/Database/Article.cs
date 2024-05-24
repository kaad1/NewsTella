using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsTella.Models.Database
{
    public class Article
    {
        public int Id { get; set; }
        public DateTime DateStamp { get; set; } = DateTime.Now;

		[Required]
		[StringLength(200)]
		[Display(Name = "Link Text")]
		public string LinkText { get; set; } = string.Empty;

		[Required]
		[StringLength(200)]
		[Display(Name = "Headline")]
		public string Headline { get; set; } = String.Empty;

		[Required]
		[StringLength(700)]
		[Display(Name = "Summary")]
		public string ContentSummary { get; set; } = String.Empty;

		[Required]
		[StringLength(4000)]
		[Display(Name = "Content")]
		public string Content { get; set; } = String.Empty;
        public int Views { get; set; }
        public int Likes { get; set; }
        public string ImageLink { get; set; } = "Default.jpg";

		[NotMapped]
		[BindProperty]
		public List<string> Cathegories { get; set; }
		
		public string Category { get; set; } = "Local";
		public string Status { get; set; } = "Draft";

    }
}
