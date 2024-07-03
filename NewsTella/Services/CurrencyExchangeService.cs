using NewsTella.Models.ViewModel;
using Newtonsoft.Json;

namespace NewsTella.Services
{
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        private readonly HttpClient _httpClient;

        public CurrencyExchangeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CurrencyExchangeVM> GetExchangeRateAsync(string baseCurrency, string targetCurrency)
        {
            string apiKey = "37800abdd10bd48c6ed1ae3df201899b";  // Replace with your actual API key
            string url = $"https://api.exchangerate-api.com/v4/latest/{baseCurrency}";  // Replace with the actual API endpoint

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                dynamic exchangeData = JsonConvert.DeserializeObject(json);

                return new CurrencyExchangeVM
                {
                    BaseCurrency = baseCurrency,
                    TargetCurrency = targetCurrency,
                    ExchangeRate = exchangeData.rates[targetCurrency]
                };
            }
            else
            {
                throw new HttpRequestException("Error fetching exchange rate data");
            }
        }
    }
}
