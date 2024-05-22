using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Data;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;
using NewsTella.Services;

using System.Collections.Generic;
using System.Linq;

namespace NewsTella.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionTypeService _subscriptionTypeService;
        private readonly AppDbContext _context;

        public SubscriptionController(ISubscriptionTypeService subscriptionTypeService, AppDbContext context)
        {
            _subscriptionTypeService = subscriptionTypeService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_subscriptionTypeService.GetSubscriptionTypes());
        }
        //[Authorize]
        public IActionResult BuySubscription()
        {
            var subscriptions = _context.SubscriptionTypes
                .Where(st => !st.IsDeleted)
                .ToList();

            return View(subscriptions);
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SubscriptionType subscriptionType)
        {
            if (ModelState.IsValid)
            {
                _subscriptionTypeService.AddSubscriptionType(subscriptionType);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            var subscriptionType = _subscriptionTypeService.GetSubscriptionTypeById(id);
            return View(subscriptionType);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(SubscriptionType subscriptionType)
        {
            _subscriptionTypeService.RemoveSubscriptionType(subscriptionType);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var subscriptionType = _subscriptionTypeService.GetSubscriptionTypeById(id);
            return View(subscriptionType);
        }

        [HttpPost]
        public IActionResult Edit(SubscriptionType subscriptionType)
        {
            _subscriptionTypeService.UpdateSubscriptionType(subscriptionType);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
