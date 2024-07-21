using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Data;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;
using PagedList;
using X.PagedList;


namespace NewsTella.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;


        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;

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

        [HttpGet]
        public async Task<IActionResult> Index(string email, int? page)
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
                var allUsers = _userManager.Users.Where(u => !u.IsDeleted).ToList();
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

            int pageSize = 8; // Number of users per page
            int pageNumber = (page ?? 1); // Default to first page

            var pagedUsers = users.ToPagedList(pageNumber, pageSize);
            return View(pagedUsers); // Return IPagedList<UserVM>
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserProfileVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfileImageUrl = user.ProfileImageUrl,
                ProfileImage = user.ProfileImage,

            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserProfile(UserProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.ProfileImageUrl = model.ProfileImageUrl;
            user.ProfileImage = model.ProfileImage;

            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                var filePath = Path.Combine("wwwroot/images/profiles", model.ProfileImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }
                user.ProfileImageUrl = $"/images/profiles/{model.ProfileImage.FileName}";
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("UserProfile", new { Message = "Your profile has been updated" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> UserProfileEdit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserProfileVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfileImageUrl = user.ProfileImageUrl,
                ProfileImage = user.ProfileImage,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserProfileEdit(UserProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.ProfileImageUrl = model.ProfileImageUrl;
            user.ProfileImage = model.ProfileImage;

            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                var filePath = Path.Combine("wwwroot/images/profiles", model.ProfileImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }
                user.ProfileImageUrl = $"/images/profiles/{model.ProfileImage.FileName}";
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("UserProfile", new { Message = "Your profile has been updated" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> SaveProfileChanges(UserProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.ProfileImageUrl = model.ProfileImageUrl;
            user.ProfileImage = model.ProfileImage;

            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                var filePath = Path.Combine("wwwroot/images/profiles", model.ProfileImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }
                user.ProfileImageUrl = $"/images/profiles/{model.ProfileImage.FileName}";
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("UserProfile", new { Message = "Your profile has been updated" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);

        }
        //    [HttpPost]
        //    public IActionResult SaveProfileChanges(UserProfileVM model)
        //    {
        //        if (ModelState.IsValid)
        //        {

        //            return PartialView("_UserProfile", model);
        //        }

        //        return PartialView("_EditUserProfile", model);
        //    }
    }


}

//public async Task<IActionResult> MyProfile(string userId)
//{
//    var user = await _userManager.FindByIdAsync(userId);
//    if (user == null)
//    {
//        return NotFound();
//    }

//    var userRoles = await _userManager.GetRolesAsync(user);

//    var model = new UserEditVM
//    {
//        UserId = user.Id,
//        FirstName = user.FirstName,
//        LastName = user.LastName,
//        Email = user.Email,
//        Roles = _roleManager.Roles.Select(r => r.Name).ToList(),
//        SelectedRoles = userRoles.ToList()
//    };
//    return View(model);
//}

//[HttpPost]
//public async Task<IActionResult> MyProfile(UserEditVM model)
//{
//    var user = await _userManager.FindByIdAsync(model.UserId);
//    if (user == null)
//    {
//        return NotFound();
//    }

//    user.FirstName = model.FirstName;
//    user.LastName = model.LastName;
//    user.Email = model.Email;
//    var result = await _userManager.UpdateAsync(user);
//    if (!result.Succeeded)
//    {
//        ModelState.AddModelError("", "Error updating user");
//        return View(model);
//    }

//    var userRoles = await _userManager.GetRolesAsync(user);
//    var rolesToAdd = model.SelectedRoles.Except(userRoles).ToList();
//    var rolesToRemove = userRoles.Except(model.SelectedRoles).ToList();

//    result = await _userManager.AddToRolesAsync(user, rolesToAdd);
//    if (!result.Succeeded)
//    {
//        ModelState.AddModelError("", "Error adding roles");
//        return View(model);
//    }

//    result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
//    if (!result.Succeeded)
//    {
//        ModelState.AddModelError("", "Error removing roles");
//        return View(model);
//    }

//    return RedirectToAction("Index");
//}








