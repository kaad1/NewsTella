namespace NewsTella.Models.ViewModel
{
    public class ElectricityPriceVM
    {
        public string Country { get; set; }
        public List<AreaPrice> AreaPrices { get; set; }
    }

    public class AreaPrice
    {
        public string AreaName { get; set; }
        public decimal Price { get; set; }
    }
}
