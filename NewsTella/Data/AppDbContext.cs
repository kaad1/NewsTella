using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsTella.Models.Database;

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Article>().ToTable("Articles");
        }
       
    }
}
