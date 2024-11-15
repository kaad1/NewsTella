﻿using Microsoft.EntityFrameworkCore;
using NewsTella.Data;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;

namespace NewsTella.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<SubscriptionService> _logger;

        public SubscriptionService(AppDbContext db, ILogger<SubscriptionService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public List<Subscription> GetSubscriptions()
        {
            var obj = _db.Subscriptions.Include(s => s.User).Include(s => s.SubscriptionType).ToList();
            return obj;
        }

        public SubscriptionType GetSubscriptionTypeByUserId(string userId)
        {
            var subscriptionType = _db.Subscriptions
                .Include(s => s.SubscriptionType)
                .Where(s => !s.IsDeleted && s.User.Id == userId)
                .Select(s => s.SubscriptionType)
                .FirstOrDefault();

            return subscriptionType;
        }

        public List<Subscription> GetSubscriptionsCloserToExpire()
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            var subscriptions = _db.Subscriptions
                .Include(s => s.User)
                .Where(s => s.Expires.AddDays(-3) < currentDate) // before two days -2
                .GroupBy(s => s.User)
                .Select(g => g.OrderBy(s => s.Expires).FirstOrDefault())
                .ToList();

            return subscriptions;
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

        public List<Subscription> FindByEmailAsync(string email)
        {
            var obj = _db.Subscriptions.Include(s => s.User).Include(s => s.SubscriptionType).
                                        Where(s => s.User.Email.Contains(email)).ToList();
            return obj;
        }
    }
}
