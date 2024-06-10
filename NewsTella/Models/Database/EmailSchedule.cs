namespace NewsTella.Models.Database
{
    public class EmailSchedule
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime ScheduledTime { get; set; }
        public bool IsSent { get; set; }
    }
}
