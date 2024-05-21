using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;

namespace NewsTella.Services
{
    public interface ISubscriptionTypeService
    {
        List<SubscriptionType> GetSubscriptionTypes();

        SubscriptionType GetSubscriptionTypeById(int id);

        void AddSubscriptionType(SubscriptionType subscriptionType);

        void RemoveSubscriptionType(SubscriptionType subscriptionType);

        void UpdateSubscriptionType(SubscriptionType subscriptionType);

    }
}
