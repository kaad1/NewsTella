namespace NewsTella.Models.Database
{
    public class SubscriptionType
    {
        public int Id { get; set; }

        public string TypeName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsDeleted { get; set; } = false;

        //public bool IsActive { get; set; } = true;

    }
}
