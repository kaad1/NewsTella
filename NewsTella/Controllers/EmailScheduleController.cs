using Microsoft.AspNetCore.Mvc;
using NewsTella.Data;
using NewsTella.Models;
using NewsTella.Models.Database;
using System;
using System.Threading.Tasks;

namespace NewsTella.Controllers
{
    public class EmailScheduleController : Controller
    {
        private readonly AppDbContext _context;

        public EmailScheduleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleEmail(string email, string subject, string body)
        {
            var scheduledTime = DateTime.Now.AddDays(27); // Schedule time 27 days from now 

            var emailSchedule = new EmailSchedule
            {
                Email = email,
                Subject = subject,
                Body = body,
                ScheduledTime = scheduledTime,
                IsSent = false
            };

            _context.EmailSchedules.Add(emailSchedule);
            await _context.SaveChangesAsync();

            return Ok("Email scheduled successfully for 30 days from now.");
        }
    }
}
