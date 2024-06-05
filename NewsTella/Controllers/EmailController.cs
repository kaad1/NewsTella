using Microsoft.AspNetCore.Mvc;
using NewsTella.Models;
using System.Net.Mail;

namespace NewsTella.Controllers
{
    public class EmailController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmailEntity objEmailParameters)
        {
            var myAppConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var Username = myAppConfig.GetValue<string>("EmailConfig:Username");
            var Password = myAppConfig.GetValue<string>("EmailConfig:Password");
            var Host = myAppConfig.GetValue<string>("EmailConfig:Host");
            var Port = myAppConfig.GetValue<int>("EmailConfig:Port");
            var FromEmail = myAppConfig.GetValue<string> ("EmailConfig:FromEmail");

            MailMessage message = new MailMessage();
            message.From = new MailAddress(FromEmail);
            message.To.Add(objEmailParameters.ToEmailAddress.ToString());
            message.Subject = objEmailParameters.Subject;
            message.IsBodyHtml = true;
            message.Body = objEmailParameters.EmailBody;

            SmtpClient mailClient = new SmtpClient(Host);
            try
            {
               
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = new System.Net.NetworkCredential(Username, Password);
                mailClient.Host = Host;
              //  mailClient.Port = Port;
                mailClient.Send(message);
                ViewBag.Message = "Email Sent Successfully !!!!";
            }
            catch (Exception ex) 
            {
                ViewBag.Message = "Faild Sending Email";
            }
            finally
            {
                mailClient.Dispose();
            }
            return View(objEmailParameters);
        }
    }
}
