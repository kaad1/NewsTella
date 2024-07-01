using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsTella.Models.Database
{
    public class Article
    {
        public int Id { get; set; }
        public DateTime DateStamp { get; set; } = DateTime.Now;

        public User User { get; set; }

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

		public virtual ICollection<Category> Categories { get; set; }

		[NotMapped]
		public string TestImage { get; set; } = string.Empty;

		public bool IsDeleted { get; set; } 

        public string Status { get; set; } = "Draft"; //default value

        public Article()
        {
            Categories = new HashSet<Category>();
        }
        public bool IsEditorsChoice { get; set; } = false;

        public DateTime DatePublished { get; set; }

        public bool IsArchived { get; set; } = false;
    }
}
