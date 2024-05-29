using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;

namespace NewsTella.Data
{
	public class AppDbContext : IdentityDbContext<User>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public DbSet<Article> Articles { get; set; }   
        public DbSet<Subscription> Subscriptions { get; set; }
        public List<Article> ArticlesList { get; set; } = new List<Article>();
		public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Article>().ToTable("Articles");
        }
	    public DbSet<NewsTella.Models.ViewModel.CategoryEditVM> CategoryEditVM { get; set; } = default!;
       
    }
}



