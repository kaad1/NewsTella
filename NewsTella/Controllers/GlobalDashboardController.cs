using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.ViewModel;
using NewsTella.Services;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewsTella.Controllers
{
    public class GlobalDashboardController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWeatherService _weatherService;

        public GlobalDashboardController(IWeatherService weatherService)
        {
            _httpClient = new HttpClient();
            _weatherService = weatherService;
        }

        public async Task<IActionResult> Detail()
        {
            GlobalVM globalVM = new GlobalVM();

            string city = "linköping,401";
            var weatherVM  = await _weatherService.GetWeatherAsync(city);
            globalVM.WeatherVM = weatherVM;



            return View(globalVM);
        }
    }
}
