using Microsoft.AspNetCore.Identity;
using NewsTella.Models.Database;

namespace NewsTella.Controllers
{
    public class UserCategory
    {
        public string UserId { get; set; }
        public Category Category { get; set; }
    }
}
