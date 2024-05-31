using NewsTella.Models.Database;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewsTella.Models.ViewModel
{
    public class SubscriptionVM
    {
        [Required]
        public int SubscriptionTypeId { get; set; }
        //public int UserId { get; set; }
        public string SubscriptionType { get; set; }
        public decimal Price { get; set; }
        public DateOnly Created { get; set; }
        public DateOnly Expires { get; set; }
        public string Description { get; set; }
        [Required]
        [Display(Name = "I agree to the terms and conditions")]
        public bool AcceptTerms { get; set; }
    }
}
