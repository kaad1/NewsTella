using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;

namespace NewsTella.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var users = _userManager.Users.ToList();
            var userVMList = new List<UserVM>();

            foreach (var user in users)
            {
                var userVm = new UserVM
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                };
                userVMList.Add(userVm);
            }
            return View(userVMList);
        }

        public async Task<IActionResult> CreateUser()
        {
            var existingUser = await _userManager.FindByEmailAsync("user@localhost");
            if (existingUser != null)
            {
                await _userManager.DeleteAsync(existingUser);
            }


            var user = new User
            {
                UserName = "user@localhost",
                Email = "user@localhost",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, "password");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Member.ToString());

                return RedirectToAction(nameof(IndexAsync));
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> UserInfor()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound();
            }
            return View();
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

        

        


        

