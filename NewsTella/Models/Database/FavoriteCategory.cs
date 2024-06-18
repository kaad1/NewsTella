using Microsoft.AspNetCore.Identity;

namespace NewsTella.Models.Database
{
    public class FavoriteCategory
    {
        public string UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
