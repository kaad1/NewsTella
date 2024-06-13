using System.ComponentModel.DataAnnotations;

namespace NewsTella.Models.Database
{
    public class Subscription
    {
        public int Id { get; set; }

        public SubscriptionType SubscriptionType { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public DateOnly Created { get; set; }

        public DateOnly Expires { get; set; }

        public User User { get; set; }

        public bool PaymentComplete { get; set; }

        public DateTime RenewalEmailSentTime { get; set; }

        public bool IsDeleted { get; set; } = false;

		//public bool IsActive { get; set; } = true;

	}
}
