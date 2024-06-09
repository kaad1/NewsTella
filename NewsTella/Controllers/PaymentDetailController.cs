using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using NewsTella.Models.Database;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NewsTella.Data;
using NewsTella.Models.ViewModel;
using NewsTella.Services;

namespace NewsTella.Controllers
{
    public class PaymentDetailController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly AppDbContext _context;
		private readonly IPaymentDetailService _paymentDetailService;

		public PaymentDetailController(UserManager<User> userManager, AppDbContext context, IPaymentDetailService paymentDetailService)
		{
			_userManager = userManager;
			_context = context;
			_paymentDetailService = paymentDetailService;
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
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(PaymentDetailVM model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.GetUserAsync(User);
				if (user == null)
				{
					return NotFound("User not found");
				}

				var paymentDetails = new PaymentDetail
				{
					CardType = model.CardType,
					NameOnCard = model.NameOnCard,
					CardNumber = model.CardNumber,
					ExpireDate = model.ExpireDate,
					CardSecurityCode = model.CardSecurityCode,
					Address = model.Address,
					City = model.City,
					State = model.State,
					PostalCode = model.PostalCode,
					Country = model.Country,
					UserId = user.Id,
            };

				
				_context.PaymentDetails.Add(paymentDetails);
				await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Thank you for Subscribing to NewsTella";

                return RedirectToAction("Index", "PaymentDetail");
			}

			return View(model);
		}
	}
}
