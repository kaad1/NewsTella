using Microsoft.EntityFrameworkCore;
using NewsTella.Data;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;

namespace NewsTella.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly AppDbContext _db;
        private readonly AppDbContext _context;
        private readonly ILogger<SubscriptionService> _logger;

        public SubscriptionService(AppDbContext db, AppDbContext context, ILogger<SubscriptionService> logger)
        {
            _db = db;
            _context = context;
            _logger = logger;
        }
        public List<Subscription> GetSubscriptions()
        {
            var obj = _db.Subscriptions.Include(s => s.User).Include(s => s.SubscriptionType).ToList();
            return obj;
        }
        public Subscription GetSubscriptionById(int id)
        {
            var subscription = _db.Subscriptions.FirstOrDefault(s => s.Id == id);
            return subscription;
        }

        public void AddSubscription(Subscription subscription)
        {
            _db.Subscriptions.Add(subscription);
            _db.SaveChanges();
            _logger.LogInformation("Subscription added with ID: {Id}", subscription.Id);
        }

        public void RemoveSubscription(Subscription subscription)
        {
            _db.Subscriptions.Remove(subscription);
            _db.SaveChanges();
        }

        public void UpdateSubscription(Subscription subscription)
        {
            _db.Subscriptions.Update(subscription);
            _db.SaveChanges();
        }
    }
}
