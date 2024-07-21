using System.Net.Http;
using System.Threading.Tasks;
using NewsTella.Models.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace NewsTella.Services
{
    public class ElectricityPriceService : IElectricityPriceService
    {
        private readonly HttpClient _httpClient;

        public ElectricityPriceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ElectricityPriceVM> GetElectricityPriceAsync(string country)
        {
            string url = $"https://spotfunc.azurewebsites.net/api/SpotPriceRequest?code=vgUdbbCJSApniy7OgY2tfEJuTomMaNzZ-QWTNcMYS8h-AzFuS91H_w==";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var priceData = JsonConvert.DeserializeObject<JObject>(json);

                var todaysSpotPrices = priceData["todaysSpotPrices"];
                var areaPrices = new List<AreaPrice>();

                foreach (var spotData in todaysSpotPrices.Last()["spotData"])
                {
                    string priceString = spotData["price"].Value<string>();
                    // Ensure correct parsing of the price with comma as decimal separator
                    decimal price = decimal.Parse(priceString, new CultureInfo("sv-SE"));

                    areaPrices.Add(new AreaPrice
                    {
                        AreaName = spotData["areaName"].Value<string>(),
                        Price = price
                    });
                }

                return new ElectricityPriceVM
                {
                    Country = country,
                    AreaPrices = areaPrices
                };
            }
            else
            {
                throw new HttpRequestException("Error fetching electricity price data");
            }
        }
    }
}
