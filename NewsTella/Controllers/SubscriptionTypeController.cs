using Microsoft.AspNetCore.Mvc;
using NewsTella.Data;
using NewsTella.Migrations;
using NewsTella.Models.Database;
using NewsTella.Services;

namespace NewsTella.Controllers
{
    public class SubscriptionTypeController : Controller
    {
        private readonly ISubscriptionTypeService _subscriptionTypeService;
        private readonly AppDbContext _context;
        
        public SubscriptionTypeController(ISubscriptionTypeService subscriptionTypeService, 
                                          AppDbContext context)
        {
            _subscriptionTypeService = subscriptionTypeService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_subscriptionTypeService.GetSubscriptionTypes());
        }
        
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
            subscriptionType = _subscriptionTypeService.GetSubscriptionTypeById(subscriptionType.Id);
            subscriptionType.IsDeleted = true;
            _subscriptionTypeService.UpdateSubscriptionType(subscriptionType);
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
    }
}
