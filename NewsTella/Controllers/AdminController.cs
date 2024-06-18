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
            RecentArticles = _context.Articles.OrderByDescending(a => a.DateStamp).Take(6).ToList()
        };

        return View(dashboardViewModel);
    }
        //admin
}
