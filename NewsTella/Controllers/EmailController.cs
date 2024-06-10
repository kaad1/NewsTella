using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NewsTella.Models;
using NewsTella.Models.Database;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Security.Policy;
using System.Text.Encodings.Web;

namespace NewsTella.Controllers
{
    //This is used for send email through the UI
    public class EmailController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public EmailController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
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
