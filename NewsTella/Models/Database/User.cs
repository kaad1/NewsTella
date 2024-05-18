using Microsoft.AspNetCore.Identity;

namespace NewsTella.Models.Database
{
	public class User:IdentityUser

	{
		public string FirstName {  get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;
	}
}
