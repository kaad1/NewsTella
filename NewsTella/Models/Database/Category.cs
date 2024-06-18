namespace NewsTella.Models.Database
{
	public class Category
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public bool IsDeleted { get; set; } = false;

		public virtual ICollection<Article> Articles { get; set; }

        public virtual ICollection<User> SubscribedUsers { get; set; }
    }
}

