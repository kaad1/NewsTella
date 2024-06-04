using Microsoft.EntityFrameworkCore;
using NewsTella.Data;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;

namespace NewsTella.Services
{
    public class SubscriptionTypeService: ISubscriptionTypeService
    {
        private readonly AppDbContext _db;

        public SubscriptionTypeService(AppDbContext db)
        {
            _db = db;
        }
        public List<SubscriptionType> GetSubscriptionTypes()
        {
            var subscriptionType = _db.SubscriptionTypes.Where(s => s.IsDeleted == false).ToList();
            return subscriptionType;
            //var obj = _db.SubscriptionTypes.ToList();
            //return obj;
        }
        public SubscriptionType GetSubscriptionTypeById(int id)
        {
            var subscriptionType = _db.SubscriptionTypes.FirstOrDefault(s => s.Id == id);
            return subscriptionType;
        }

        public void AddSubscriptionType(SubscriptionType subscriptionType)
        {
            _db.SubscriptionTypes.Add(subscriptionType);
            _db.SaveChanges();
        }

        public void RemoveSubscriptionType(SubscriptionType subscriptionType)
        {
            _db.SubscriptionTypes.Remove(subscriptionType);
            _db.SaveChanges();
        }

        public void UpdateSubscriptionType(SubscriptionType subscriptionType)
        {
            _db.SubscriptionTypes.Update(subscriptionType);
            _db.SaveChanges();
        }
    }
}
