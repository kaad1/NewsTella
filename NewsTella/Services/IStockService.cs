using Azure.Storage.Blobs.Models;
using NewsTella.Models;

namespace NewsTella.Services
{
    public interface IStockService
    {
        Task<List<StockQuote>> GetMostActiveStocksAsync();
    }
}
