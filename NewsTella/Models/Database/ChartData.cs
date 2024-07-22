using Azure.Data.Tables;
using Azure;

namespace NewsTella.Models.Database
{
    public class ChartData : ITableEntity
    {
        public string RowKey { get; set; }
        public string PartitionKey { get; set; }
        public double Price { get; set; }

        // Required properties by ITableEntity
        public DateTimeOffset? Timestamp { get; set; }  // Adjusting the type to DateTimeOffset?
        public ETag ETag { get; set; }
        public string AreaName { get; set; }
    }
}
