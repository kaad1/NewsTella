using Azure.Data.Tables;
using Azure;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.ViewModel;
using NewsTella.Services;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NewsTella.Models.Database;
using Stripe;

namespace NewsTella.Controllers
{
    public class GlobalDashboardController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly ICurrencyExchangeService _currencyService;
        private readonly IElectricityPriceService _electricityPriceService;
        private readonly TableServiceClient _tableServiceClient;
        private readonly IConfiguration _configuration;

        private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=newstellastorage2405nade;AccountKey=kyxzM1QrmedAN+hKtGtVz3BqpM1al/vJ0VeMLP+xO0L6Ww4BUHtwvssvYtWcdKdTnJvK19+6pAJM+AStPoWvNA==;EndpointSuffix=core.windows.net";
        private const string tableName = "Spotprices";

        public GlobalDashboardController(IWeatherService weatherService, ICurrencyExchangeService currencyService, IElectricityPriceService electricityPriceService, IConfiguration configuration)
        {
            _configuration = configuration;
            _weatherService = weatherService;
            _currencyService = currencyService;
            _electricityPriceService = electricityPriceService;
            _tableServiceClient = new TableServiceClient(_configuration["AzureWebJobsStorage"]);
        }


        public async Task<IActionResult> Detail()
        {
            //var tableClient = new TableClient(connectionString, tableName);
            //Pageable<TableEntity> queryResults = tableClient.Query<TableEntity>();

            var tableClient = _tableServiceClient.GetTableClient(tableName);
            var queryResults = tableClient.Query<ChartData>().Where(e => e.PartitionKey == "SE3").ToList();

            List<ChartDataVM> chartDataVMList = new List<ChartDataVM>();
            foreach (var entity in queryResults)
            {
                var chartDataVM = new ChartDataVM
                {
                    Timestamp = entity.RowKey.Substring(8),
                    Prices = Convert.ToDecimal(entity.Price) / 100000,
                    PartitionKey = entity.PartitionKey
                };
                chartDataVMList.Add(chartDataVM);
            }

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
                ElectricityPriceVM = electricityPriceVM,
                ChartData = chartDataVMList
            };

            return View(globalVM);
        }
    }
}
