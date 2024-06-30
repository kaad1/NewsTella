using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsTella.Data;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;
using NewsTella.Services;
using X.PagedList;
using System.Collections.Generic;
using System.Linq;
using NewsTella.Helpers;
using Microsoft.AspNetCore.Http;

namespace NewsTella.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionTypeService _subscriptionTypeService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public SubscriptionController(ISubscriptionTypeService subscriptionTypeService, 
                                      AppDbContext context, 
                                      ISubscriptionService subscriptionService,  
                                      UserManager<User> userManager)
        {
            _subscriptionTypeService = subscriptionTypeService;
            _subscriptionService = subscriptionService;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index(string email, int? page)
        {
            List<Subscription> subscriptions = new List<Subscription>();

            if (!string.IsNullOrEmpty(email))
            {
                subscriptions = _subscriptionService.FindByEmailAsync(email);
            }
            else 
            {
               subscriptions = _subscriptionService.GetSubscriptions();
            }
                
            int pageSize = 8; // Number of users per page
            int pageNumber = (page ?? 1); // Default to first page

            var pagedSubscriptions = subscriptions.ToPagedList(pageNumber, pageSize);
            return View(pagedSubscriptions); // Return IPagedList<UserVM>
        }


        [Authorize]
        public IActionResult SelectSubscription()
        {
            var subscriptions = _context.SubscriptionTypes
                .Where(st => !st.IsDeleted)
                .ToList();
            return View(subscriptions);
        }

        public IActionResult Create(int subscriptionTypeId)
        {
            var subscriptionType = _subscriptionTypeService.GetSubscriptionTypeById(subscriptionTypeId);
            var model = new SubscriptionVM
            {
                SubscriptionTypeId = subscriptionTypeId,
                SubscriptionType = subscriptionType.TypeName,
                Price = subscriptionType.Price,
                Created = DateOnly.FromDateTime(DateTime.Now),
                Expires = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                Description = subscriptionType.Description,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(SubscriptionVM model)
        {
            if (ModelState.IsValid)
            {
                var subscriptionType = _subscriptionTypeService.GetSubscriptionTypeById(model.SubscriptionTypeId);
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                Subscription subscription = new Subscription
                {
                    SubscriptionType = subscriptionType,
                    Price = subscriptionType.Price,
                    Created = DateOnly.FromDateTime(DateTime.Now),
                    Expires = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                    PaymentComplete = false,
                    User = user
                };
                _subscriptionService.AddSubscription(subscription);
                HttpContext.Session.Set<Subscription>("SubscriptionToPay", subscription);
                return RedirectToAction("Create", "PaymentDetail");
            }
            return View(model);
        }
    }
}
