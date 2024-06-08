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

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailConfig = _configuration.GetSection("EmailConfig");
        var username = emailConfig["Username"];
        var password = emailConfig["Password"];
        var host = emailConfig["Host"];
        var port = int.Parse(emailConfig["Port"]);
        var fromEmail = emailConfig["FromEmail"];

        var message = new MailMessage();
        message.From = new MailAddress(fromEmail);
        message.To.Add(email);
        message.Subject = subject;
        message.IsBodyHtml = true;
        message.Body = htmlMessage;

        using (var smtpClient = new SmtpClient(host, port))
        {
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(username, password);

            await smtpClient.SendMailAsync(message);
        }
    }
}
