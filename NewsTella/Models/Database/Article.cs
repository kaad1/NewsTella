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
		public string Headline { get; set; } = string.Empty;

		[Required]
		[StringLength(700)]
		[Display(Name = "Summary")]
		public string ContentSummary { get; set; } = string.Empty;

		[Required]
		[StringLength(4000)]
		[Display(Name = "Content")]
		public string Content { get; set; } = string.Empty;
        public int Views { get; set; }
        public int Likes { get; set; }
		public string ImageLink { get; set; } = string.Empty;

		[NotMapped]
		public IFormFile FormImage { get; set; }


		[NotMapped]
		[BindProperty]
		public List<string> Cathegories { get; set; } = [];		
		

		public string Category { get; set; } = string.Empty;

		[NotMapped]
		public string TestImage { get; set; } = string.Empty;

		public bool IsDeleted { get; set; } = false;

        public string Status { get; set; } = "Draft";

    }
}
