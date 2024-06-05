using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NewsTella.Models;
using System;
using System.Net.Mail;

namespace NewsTella.Controllers
{
    public class EmailController : Controller
    {
        private readonly IConfiguration _configuration;

        public EmailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmailEntity objEmailParameters)
        {
            var emailConfig = _configuration.GetSection("EmailConfig");
            var Username = emailConfig["Username"];
            var Password = emailConfig["Password"];
            var Host = emailConfig["Host"];
            var Port = int.Parse(emailConfig["Port"]);
            var FromEmail = emailConfig["FromEmail"];

            MailMessage message = new MailMessage();
            message.From = new MailAddress(FromEmail);
            message.To.Add(objEmailParameters.ToEmailAddress);
            message.Subject = objEmailParameters.Subject;
            message.IsBodyHtml = true;
            message.Body = objEmailParameters.EmailBody;

            SmtpClient mailClient = new SmtpClient(Host, Port);
            mailClient.EnableSsl = true; // Ensure SSL is enabled for Gmail
            mailClient.UseDefaultCredentials = false;
            mailClient.Credentials = new System.Net.NetworkCredential(Username, Password);

            try
            {
                mailClient.Send(message);
                ViewBag.Message = "Email Sent Successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Failed to send email. Error: {ex.Message}";
            }

            return View(objEmailParameters);
        }
    }
}
