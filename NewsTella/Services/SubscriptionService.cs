using Microsoft.EntityFrameworkCore;
using NewsTella.Data;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;

namespace NewsTella.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly AppDbContext _db;

        public SubscriptionService(AppDbContext db)
        {
            _db = db;
        }
        public List<Subscription> GetSubscriptions()
        {
            var obj = _db.Subscriptions.ToList();
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
