using NewsTella.Models.ViewModel;

namespace NewsTella.Services
{
    public interface IWeatherService
    {
        Task<WeatherVM> GetWeatherAsync(string city);
    }
}
