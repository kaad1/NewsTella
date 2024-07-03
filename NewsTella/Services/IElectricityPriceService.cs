using NewsTella.Models.ViewModel;

namespace NewsTella.Services
{
    public interface IElectricityPriceService
    {
        Task<ElectricityPriceVM> GetElectricityPriceAsync(string country);
    }
}
