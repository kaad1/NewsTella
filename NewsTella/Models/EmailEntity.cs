using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NewsTella.Models
{
    public class EmailEntity
    {
        [ValidateNever]
        public string FromEmailAddress { get; set; }

        public string ToEmailAddress { get; set; }

        public string Subject { get; set; }

        public string EmailBody { get; set; }
    }
}
