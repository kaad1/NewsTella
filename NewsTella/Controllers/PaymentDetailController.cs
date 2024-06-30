using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using NewsTella.Models.Database;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NewsTella.Data;
using NewsTella.Models.ViewModel;
using NewsTella.Services;
using Azure.Core;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Stripe;
using Microsoft.Extensions.Options;
using NewsTella.Helpers;

namespace NewsTella.Controllers
{
    public class PaymentDetailController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly IPaymentDetailService _paymentDetailService;
		private readonly IEmailSenderService _emailSenderService;
		private readonly ISubscriptionService _subscriptionService;
        private readonly IOptions<StripeSettings> _stripeSettings;

        public PaymentDetailController(UserManager<User> userManager, 
									  ISubscriptionService subscriptionService,
									  IPaymentDetailService paymentDetailService, 
									  IEmailSenderService emailSenderService, 
									  IOptions<StripeSettings> stripeSettings)
		{
			_userManager = userManager;
			_subscriptionService = subscriptionService;
			_paymentDetailService = paymentDetailService;
			_emailSenderService = emailSenderService;
			_stripeSettings = stripeSettings;
		}

        public IActionResult FinishSubscriptionProcess()
        {
            return View();
        }

        public IActionResult Index()
		{
			return View(_paymentDetailService.GetPaymentDetails());
		}

		[HttpGet]
		public IActionResult Create()
		{
			var subscription = HttpContext.Session.Get<Models.Database.Subscription>("SubscriptionToPay");
            return View(subscription);
		}

		//[HttpPost]
		//public async Task<IActionResult> Create(PaymentDetailVM model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		
		//		if (user == null)
		//		{
		//			return NotFound("User not found");
		//		}

		//		var paymentDetails = new PaymentDetail
		//		{
		//			CardType = model.CardType,
		//			NameOnCard = model.NameOnCard,
		//			CardNumber = model.CardNumber,
		//			ExpireDate = model.ExpireDate,
		//			CardSecurityCode = model.CardSecurityCode,
		//			Address = model.Address,
		//			City = model.City,
		//			State = model.State,
		//			PostalCode = model.PostalCode,
		//			Country = model.Country,
		//			UserId = user.Id,
  //              };
  //              _context.PaymentDetails.Add(paymentDetails);
  //              await _context.SaveChangesAsync();
		//		
  //              TempData["AlertMessage"] = "Thank you for Subscribing to NewsTella";

  //              return RedirectToAction("Index", "PaymentDetail");
		//	}

		//	return View(model);
		//}

        [HttpPost]
        public async Task<IActionResult> Create(string stripeEmail, string stripeToken)
		{
            var user = await _userManager.GetUserAsync(User);
            await _emailSenderService.SendEmailAsync(user.Email, "NewsTella new subscription", "Thank you for subscribing NewsTella");
			var subscription = HttpContext.Session.Get<Models.Database.Subscription>("SubscriptionToPay");
           
			subscription = _subscriptionService.GetSubscriptionById(subscription.Id);
            subscription.PaymentComplete = true;
            _subscriptionService.UpdateSubscription(subscription);
            
			HttpContext.Session.Remove("SubscriptionToPay");
            return RedirectToAction("Success", "PaymentDetail");
        }

		public IActionResult Success()
		{
            TempData["AlertMessage"] = "Payment successful!";
            return View();
		}

        [HttpPost]
        public IActionResult Charge(string stripeToken)
        {
            try
            {
                var options = new ChargeCreateOptions
                {
                    Amount = 500, // Amount in cents
                    Currency = "usd",
                    Description = "Sample Charge",
                    Source = stripeToken,
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);

                TempData["AlertMessage"] = "Payment successful!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "Payment failed: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
