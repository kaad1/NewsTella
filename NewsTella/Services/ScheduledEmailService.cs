using Microsoft.Extensions.Hosting;
using NewsTella.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

public class ScheduledEmailService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceProvider _serviceProvider;

    public ScheduledEmailService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Calculate the time until the next 3 PM
        var now = DateTime.Now;
        var scheduledTime = new DateTime(now.Year, now.Month, now.Day, 21, 41,41);

        if (now > scheduledTime)
        {
            scheduledTime = scheduledTime.AddDays(1);
        }

        var initialDelay = scheduledTime - now;

        // Set the timer to execute DoWork method once after initial delay and then every 24 hours
        _timer = new Timer(DoWork, null, initialDelay, TimeSpan.FromHours(24));

        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSenderService>();

            // Replace with your logic to retrieve emails and send them
            string email = "nadeehcc@gmail.com";
            string subject = "Scheduled Email";
            string message = "This is a scheduled email.";

            await emailSender.SendEmailAsync(email, subject, message);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
