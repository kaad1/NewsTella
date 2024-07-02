using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.ViewModel;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewsTella.Controllers
{
    public class WeatherController : Controller
    {
        private readonly HttpClient _httpClient;

        public WeatherController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Detail()
        {
            string appid = "37800abdd10bd48c6ed1ae3df201899b";
            string city = "linköping,401";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={appid}&units=metric";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                dynamic weatherData = JsonConvert.DeserializeObject(json);

                WeatherVM weatherVM = new WeatherVM
                {
                    City = weatherData.name,
                    Country = weatherData.sys.country,
                    Temperature = weatherData.main.temp,
                    FeelsLike = weatherData.main.feels_like,
                    MinTemperature = weatherData.main.temp_min,
                    MaxTemperature = weatherData.main.temp_max,
                    Pressure = weatherData.main.pressure,
                    Humidity = weatherData.main.humidity,
                    Weather = weatherData.weather[0].main,
                    WeatherDescription = weatherData.weather[0].description,
                    WindSpeed = weatherData.wind.speed,
                    WindDirection = weatherData.wind.deg,
                    Cloudiness = weatherData.clouds.all
                };

                return View(weatherVM);
            }
            else
            {
                // Handle error response here
                return View("Error");
            }
        }
    }
}
