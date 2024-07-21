namespace NewsTella.Models.ViewModel
{
    public class WeatherHistoryData
    {
        public string PartitionKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public decimal MinTemperature { get; set; }

        public decimal Humidity { get; set; }
    }
}
