using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;

namespace NewsTella.Data
{
	public class AppDbContext : IdentityDbContext<User>
	{
        internal object _articleservice;

        public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public DbSet<Article> Articles { get; set; }   
        public DbSet<Subscription> Subscriptions { get; set; }
		public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }




 
        public DbSet<EmailSchedule> EmailSchedules { get; set; }
        public DbSet<FavoriteCategory> FavoriteCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Article>()
                .HasMany(a => a.Categories)
                .WithMany(c => c.Articles)
                .UsingEntity(j => j.ToTable("ArticleCategory"));

            modelBuilder.Entity<User>()
                  .HasMany(u => u.FavoriteCategories)
                  .WithMany(c => c.SubscribedUsers)
                  .UsingEntity<FavoriteCategory>();
        }
    }
}



