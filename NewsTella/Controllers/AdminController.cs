using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NewsTella.Data;
using NewsTella.Models.ViewModel;

public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var dashboardViewModel = new DashboardViewModel
        {
            TotalUsers = _context.Users.Count(),
            TotalArticles = _context.Articles.Count(),
            TotalSubscriptions = _context.Subscriptions.Count(),
            RecentArticles = _context.Articles.OrderByDescending(a => a.DateStamp).Take(5).ToList(),
            PopularArticles = _context.Articles.OrderByDescending(a => a.Likes).Take(5).ToList(),
            ProCount = _context.Subscriptions.Count(s => s.SubscriptionType.TypeName == "Pro"),
            PremiumCount = _context.Subscriptions.Count(s => s.SubscriptionType.TypeName == "Premium"),
            BasicCount = _context.Subscriptions.Count(s => s.SubscriptionType.TypeName == "Basic")
    };

        return View(dashboardViewModel);
    }


    public ActionResult Dashboard()
    {
        var proCount = _context.Subscriptions.Count(s => s.SubscriptionType.TypeName == "Pro");
        var premiumCount = _context.Subscriptions.Count(s => s.SubscriptionType.TypeName == "Premium");
        var basicCount = _context.Subscriptions.Count(s => s.SubscriptionType.TypeName == "Basic");

        var dashboardViewModel = new DashboardViewModel
        {
            ProCount = proCount,
            PremiumCount = premiumCount,
            BasicCount = basicCount
        };

        return View(dashboardViewModel);
    }
}
