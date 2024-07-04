using Microsoft.AspNetCore.Mvc;
using NewsTella.Models;
using NewsTella.Services;

namespace NewsTella.Controllers
{
    public class StockController : Controller
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        public async Task<IActionResult> Index()
        {
            List<StockQuote> mostActiveStocks = await _stockService.GetMostActiveStocksAsync();
            return View(mostActiveStocks);
        }
    }
}
