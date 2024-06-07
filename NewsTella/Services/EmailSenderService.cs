using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using NewsTella.Services;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSenderService : IEmailSenderService
{
    private readonly IConfiguration _configuration;

    public EmailSenderService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailConfig = _configuration.GetSection("EmailConfig");
        var smtpClient = new SmtpClient(emailConfig["Host"], int.Parse(emailConfig["Port"]))
        {
            Credentials = new System.Net.NetworkCredential(emailConfig["Username"], emailConfig["Password"]),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailConfig["FromEmail"]),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        return smtpClient.SendMailAsync(mailMessage);
    }
}
