using Newtonsoft.Json;

namespace NewsTella.Models
{
    public class StockQuote
    {
            public string Symbol { get; set; }
            public decimal LastPrice { get; set; }
            public decimal PriceChange { get; set; }
            public string PercentChange { get; set; }

    }
}
