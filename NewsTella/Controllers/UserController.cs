using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Data.Migrations;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;

namespace NewsTella.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string email)
        {
            var users = new List<UserVM>();

            if (!string.IsNullOrEmpty(email))
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    users.Add(new UserVM
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName, // Assuming you have FirstName and LastName properties
                        LastName = user.LastName,
                        Roles = await _userManager.GetRolesAsync(user)
                    });
                }
            }
            else
            {
                //var allUsers = _userManager.Users.ToList();
                var allUsers = _userManager.Users.Where(u => !u.IsDeleted).ToList(); // Exclude soft-deleted records

                foreach (var user in allUsers)
                {
                    users.Add(new UserVM
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName, // Assuming you have FirstName and LastName properties
                        LastName = user.LastName,
                        Roles = await _userManager.GetRolesAsync(user)
                    });
                }
            }

            return View(users);
        }

        

        public async Task<IActionResult> IndexAsync()
        {
            //var users = _userManager.Users.ToList();
            var users = _userManager.Users.Where(u => !u.IsDeleted).ToList(); // Exclude soft-deleted records

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

        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new UserEditVM
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = _roleManager.Roles.Select(r => r.Name).ToList(),
                SelectedRoles = userRoles.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error updating user");
                return View(model);
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = model.SelectedRoles.Except(userRoles).ToList();
            var rolesToRemove = userRoles.Except(model.SelectedRoles).ToList();

            result = await _userManager.AddToRolesAsync(user, rolesToAdd);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error adding roles");
                return View(model);
            }

            result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error removing roles");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserEditVM
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                SelectedRoles = await _userManager.GetRolesAsync(user)
            };

            return View(model);
        }

        // POST: User/SoftDelete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.IsDeleted = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error marking user as deleted");
                return View();
            }

            return RedirectToAction(nameof(Index));
        }


    }
}

        

        


        

