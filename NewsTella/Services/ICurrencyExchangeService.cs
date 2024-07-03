using NewsTella.Models.ViewModel;

namespace NewsTella.Services
{
    public interface ICurrencyExchangeService
    {
        Task<CurrencyExchangeVM> GetExchangeRateAsync(string baseCurrency, string targetCurrency);
    }
}
