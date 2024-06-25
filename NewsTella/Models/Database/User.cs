using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Contracts;

namespace NewsTella.Models.Database
{
	public class User:IdentityUser
	{
        public string FirstName {  get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Category> FavoriteCategories { get; set; }

    }
}
