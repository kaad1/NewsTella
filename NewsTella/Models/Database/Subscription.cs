namespace NewsTella.Models.Database
{
    public class Subscription
    {
        public int Id { get; set; }

        public SubscriptionType SubscriptionType { get; set; }

        public decimal Price { get; set; }

        public DateOnly Created { get; set; }

        public DateOnly Expires { get; set; }

        public User User { get; set; }

        public bool PaymentComplete { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
