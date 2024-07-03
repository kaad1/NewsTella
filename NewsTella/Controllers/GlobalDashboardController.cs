using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.ViewModel;
using NewsTella.Services;
using System.Threading.Tasks;

namespace NewsTella.Controllers
{
    public class GlobalDashboardController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly ICurrencyExchangeService _currencyService;
        private readonly IElectricityPriceService _electricityPriceService;

        public GlobalDashboardController(IWeatherService weatherService, ICurrencyExchangeService currencyService, IElectricityPriceService electricityPriceService)
        {
            _weatherService = weatherService;
            _currencyService = currencyService;
            _electricityPriceService = electricityPriceService;
        }

        public async Task<IActionResult> Detail()
        {
            string city = "linköping,401";
            string baseCurrency = "USD"; // Example base currency
            string targetCurrency = "SEK"; // Example target currency
            string country = "SE"; // Example country for electricity prices

            var weatherVM = await _weatherService.GetWeatherAsync(city);
            var exchangeRateVM = await _currencyService.GetExchangeRateAsync(baseCurrency, targetCurrency);
            var electricityPriceVM = await _electricityPriceService.GetElectricityPriceAsync(country);

            var globalVM = new GlobalVM
            {
                WeatherVM = weatherVM,
                CurrencyExchangeVM = exchangeRateVM,
                ElectricityPriceVM = electricityPriceVM
            };

            return View(globalVM);
        }
    }
}
