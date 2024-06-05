using Microsoft.AspNetCore.Identity.UI.Services;

namespace NewsTella.Helpers
{
	public class EmailHelper: IEmailSender
	{
		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var help = "";
		}
	}
}
