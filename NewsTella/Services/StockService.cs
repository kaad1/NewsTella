
using Azure.Storage.Blobs.Models;
using NewsTella.Models;
using System.Globalization;
using System.Text.Json;

namespace NewsTella.Services
{
    public class StockService : IStockService
    {
        private readonly HttpClient _httpClient;

        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<StockQuote>> GetMostActiveStocksAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://yahoo-finance15.p.rapidapi.com/api/v1/markets/options/most-active?type=STOCKS"),
                Headers =
                {
                    { "x-rapidapi-key", "e18454dc17msh2f73b7ff795e47ap11b32djsnca5a2a7a5c64" },
                    { "x-rapidapi-host", "yahoo-finance15.p.rapidapi.com" },
                },
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                using (JsonDocument doc = JsonDocument.Parse(responseBody))
                {
                    JsonElement root = doc.RootElement;
                    JsonElement body = root.GetProperty("body");
                    var stockInfoList = new List<StockQuote>();

                    foreach (JsonElement stock in body.EnumerateArray())
                    {
                        stockInfoList.Add(new StockQuote
                        {
                            Symbol = stock.GetProperty("symbol").GetString(),
                            LastPrice = decimal.Parse(stock.GetProperty("lastPrice").GetString(), CultureInfo.InvariantCulture),
                            PriceChange = decimal.Parse(stock.GetProperty("priceChange").GetString().Replace("+", "").Replace("%", ""), CultureInfo.InvariantCulture),
                            PercentChange = stock.GetProperty("percentChange").GetString()
                        });
                    }

                    return stockInfoList;
                }
            }
        }
    }
}
