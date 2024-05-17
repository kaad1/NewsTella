using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.Database;

namespace NewsTella.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _manager;

        public UserController(UserManager<User> userManager)
        {
            _manager = userManager;
        }

        public IActionResult Index()
        {
            var users = _manager.Users;
            return View(users);
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Create(User user)
        //{
        //    if (!_manager.UserExistsAsync(user.FirstName).GetAwaiter().GetResult())
        //    {
        //        _manager.CreateAsync(new User(user.LastName)).GetAwaiter().GetResult();
        //    }
        //    return RedirectToAction("Index");
        //}

    }
}

        

        


        

