using NewsTella.Models.Database;

namespace NewsTella.Services
{
    public interface IEmailSenderService
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
