using NewsTella.Models.Database;

namespace NewsTella.Models.ViewModel
{
    public class SubscriptionVM
    {
        public int SubscriptionId { get; set; }
        public string SubscriptionType { get; set; }
        public decimal Price { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public string UserName { get; set; }
        public bool PaymentComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}