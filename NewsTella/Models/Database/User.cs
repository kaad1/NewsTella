using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Contracts;

namespace NewsTella.Models.Database
{
	public class User:IdentityUser

	{
        public string FirstName {  get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;

        //public string Email {  get; set; }

        public bool IsDeleted { get; set; } = false;
       


        //public List<Article> NewsletterArticles { get; set; }

    }
}
