using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsTella.Data;
using NewsTella.Models.Database;
using NewsTella.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class EmailBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public EmailBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckAndSendEmailsAsync();
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Check every minute
        }
    }

    private async Task CheckAndSendEmailsAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var subscriptionService = scope.ServiceProvider.GetRequiredService<ISubscriptionService>();
            List<Subscription> subscriptions = subscriptionService.GetSubscriptionsCloserToExpire();
            var emailSenderService = scope.ServiceProvider.GetRequiredService<IEmailSenderService>();

            foreach (var subscription in subscriptions)
            {
                if (subscription.RenewalEmailSentTime == null || 
                    subscription.RenewalEmailSentTime <= DateTime.Now.AddDays(-1)) //To remind every day 
                {
                    subscription.RenewalEmailSentTime = DateTime.Now;
                    subscriptionService.UpdateSubscription(subscription);
                    await emailSenderService.SendEmailAsync(subscription.User.Email,
                                                            "Renew Newstella subscription",
                                                            "Please renew Newstella subscription.");
                }
            }

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            var now = DateTime.Now;
            var pendingEmails = dbContext.EmailSchedules
                .Where(e => e.ScheduledTime <= now && !e.IsSent)
                .ToList();

            foreach (var emailSchedule in pendingEmails)
            {
                await emailSenderService.SendEmailAsync(emailSchedule.Email, emailSchedule.Subject, emailSchedule.Body);
                emailSchedule.IsSent = true;
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
// To test Add days -31 (only once) and subscriptionService 