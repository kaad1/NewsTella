using Microsoft.AspNetCore.Identity.UI.Services;
using NewsTella.Services;

namespace NewsTella.Helpers
{
	public class EmailHelper: IEmailSender
	{
        private readonly IEmailSenderService _emailSenderService;

        public EmailHelper(IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return _emailSenderService.SendEmailAsync(email, subject, htmlMessage);
        }
    }
}
