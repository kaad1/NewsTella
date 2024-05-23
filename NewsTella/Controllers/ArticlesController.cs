using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsTella.Data;
using NewsTella.Models;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;
using NewsTella.Services;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
namespace NewsTella.Controllers
{
    public class ArticlesController : Controller
    {
        //private readonly AppDbContext _context;

        //public ArticlesController(AppDbContext context)
        //{
        //    _context = context;
        //}
        //// GET: Articles/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //// POST: Articles/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(ArticleVM model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var article = new Article
        //        {
        //            DateStamp = DateTime.Now,
        //            LinkText = model.LinkText,
        //            Headline = model.Headline,
        //            ContentSummary = model.ContentSummary,
        //            Content = model.Content,
        //            Views = 0,
        //            Likes = 0,
        //            ImageLink = model.ImageLink,
        //            Category = model.Category
        //        };
        //        _context.Add(article);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index)); // Or any other action
        //    }
        //    return View(model);
        //}
        //// GET: Articles/Index (to list all articles)
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Articles.ToListAsync());
        //}


    }
}
