using Azure;

namespace NewsTella.Models.ViewModel
{
    public class ChartDataVM
    {
        public string RowKey { get; set; }
        public string PartitionKey { get; set; }
        public decimal Prices { get; set; }

        // Required properties by ITableEntity
        public string Timestamp { get; set; }  // Adjusting the type to DateTimeOffset?

    }
}
