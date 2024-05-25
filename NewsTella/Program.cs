using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NewsTella.Data;
using NewsTella.Models.Database;
using NewsTella.Services;

namespace NewsTella
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(connectionString));

            //"LexiconConnection": "Data Source=dreammaker-it.se;Initial Catalog=newstelladb;User ID=newstellaadmin;Password=Nutella24!;Encrypt=False;Trust Server Certificate=True"
            //var connectionString = builder.Configuration.GetConnectionString("LexiconConnection") ?? throw new InvalidOperationException("Connection string 'LexiconConnection' not found.");
            //builder.Services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
				.AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddScoped<UserManager<User>, AppUserManager>();
            builder.Services.AddScoped<ISubscriptionTypeService, SubscriptionTypeService>();
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

            builder.Services.AddControllersWithViews();

			builder.Services.AddScoped<IArticlesService, ArticlesService>();            

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { Roles.Admin, Roles.Editor, Roles.Writer, Roles.Member };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.ToString()))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                    }
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                string adminFirstName = "Admin";
                string adminLastName = "Lexicon";
                string adminEmail = "admin@admin.com";
                string adminPassword = "Test1234,";

                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var user = new User();
                    user.FirstName = adminFirstName;
                    user.LastName = adminLastName;
                    user.UserName = adminEmail;
                    user.Email = adminEmail;

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }

                string editorFirstName = "Editor";
                string editorLastName = "Lexicon";
                string editorEmail = "editor@editor.com";
                string editorPassword = "Test1234,";

                if (await userManager.FindByEmailAsync(editorEmail) == null)
                {
                    var user = new User();
                    user.FirstName = editorFirstName;
                    user.LastName = editorLastName;
                    user.UserName = editorEmail;
                    user.Email = editorEmail;

                    await userManager.CreateAsync(user, editorPassword);

                    await userManager.AddToRoleAsync(user, Roles.Editor.ToString());
                }

                string writerFirstName = "Writer";
                string writerLastName = "Lexicon";
                string writerEmail = "writer@writer.com";
                string writerPassword = "Test1234,";

                if (await userManager.FindByEmailAsync(writerEmail) == null)
                {
                    var user = new User();
                    user.FirstName = writerFirstName;
                    user.LastName= writerLastName;
                    user.UserName = writerEmail;
                    user.Email = writerEmail;

                    await userManager.CreateAsync(user, writerPassword);

                    await userManager.AddToRoleAsync(user, Roles.Writer.ToString());
                }

                string memberFirstName = "Member";
                string memberLastName = "Lexicon";
                string memberEmail = "writer@writer.com";
                string memberPassword = "Test1234,";

                if (await userManager.FindByEmailAsync(memberEmail) == null)
                {
                    var user = new User();
                    user.FirstName = memberFirstName;
                    user.LastName = memberLastName;
                    user.UserName = memberEmail;
                    user.Email = memberEmail;

                    await userManager.CreateAsync(user, memberPassword);

                    await userManager.AddToRoleAsync(user, Roles.Member.ToString());
                }
            }

            app.Run();
		}
	}
}
