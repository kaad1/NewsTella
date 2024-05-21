using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;
using NewsTella.Services;

namespace NewsTella.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionTypeService _subscriptionTypeService;

        public SubscriptionController(ISubscriptionTypeService subscriptionTypeService)
        {
            _subscriptionTypeService = subscriptionTypeService;
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
