using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;

namespace NewsTella.Services
{
    public interface ISubscriptionService
    {
        List<Subscription> GetSubscriptions();

        List<Subscription> GetSubscriptionsCloserToExpire();

        Subscription GetSubscriptionById(int id);

        void AddSubscription(Subscription subscription);

        void RemoveSubscription(Subscription subscription);

        void UpdateSubscription(Subscription subscription);

        public List<Subscription> FindByEmailAsync(string email);

    }
}
