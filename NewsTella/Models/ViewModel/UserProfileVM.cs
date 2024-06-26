using NewsTella.Models.Database;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsTella.Models.ViewModel
{
    public class UserProfileVM
    {
        public string UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        [NotMapped]
        public IFormFile ProfileImage { get; set; } 
        public string ProfileImageUrl { get; set; } 
        public virtual ICollection<Category> FavoriteCategories { get; set; }
        
    }
}
