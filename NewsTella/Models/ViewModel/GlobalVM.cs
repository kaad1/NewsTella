namespace NewsTella.Models.ViewModel
{
    public class GlobalVM
    {
        public WeatherVM WeatherVM { get; set; }

        public ElectricityPriceVM ElectricityPriceVM { get; set;}

        public CurrencyExchangeVM CurrencyExchangeVM { get; set; }

        public List<ChartDataVM> ChartData { get; set; }
    }
}
