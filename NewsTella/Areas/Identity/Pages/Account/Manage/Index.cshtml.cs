﻿////// Licensed to the .NET Foundation under one or more agreements.
////// The .NET Foundation licenses this file to you under the MIT license.
////#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;

namespace NewsTella.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<IndexModel> _logger;       

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        ///   
        public UserProfileVM UserProfile { get; set; } // Added this property for UserProfile
        public UserProfileVM EditUserProfile { get; set; } // Added this property for EditUserProfile

        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// 
        [NotMapped]
        public IFormFile ProfileImage { get; set; }
        public string ProfileImageUrl { get; set; }
        public virtual ICollection<Category> FavoriteCategories { get; set; }

        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public IFormFile FormImage { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            

            [NotMapped]
            public IFormFile ProfileImage { get; set; }
            public string ProfileImageUrl { get; set; }
            public virtual ICollection<Category> FavoriteCategories { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };

            // Load the user profile data
            UserProfile = new UserProfileVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfileImageUrl = user.ProfileImageUrl
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}


//namespace NewsTella.Areas.Identity.Pages.Account.Manage
//{
//    public class IndexModel : PageModel
//    {
//        private readonly UserManager<User> _userManager;
//        private readonly SignInManager<User> _signInManager;

//        public UserProfileVM UserProfile { get; set; }

//        public IndexModel(UserManager<User> userManager, SignInManager<User> signInManager)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//        }

//        public string Username { get; set; }

//        [TempData]
//        public string StatusMessage { get; set; }

//        [BindProperty]
//        public InputModel Input { get; set; }

//        [NotMapped]
//        public IFormFile ProfileImage { get; set; }
//        public string ProfileImageUrl { get; set; }
//        public virtual ICollection<Category> FavoriteCategories { get; set; }

//        public class InputModel
//        {
//            [Phone]
//            [Display(Name = "Phone number")]
//            public string PhoneNumber { get; set; }
//            public IFormFile FormImage { get; set; }

//            [NotMapped]
//            public IFormFile ProfileImage { get; set; }
//            public string ProfileImageUrl { get; set; }
//            public virtual ICollection<Category> FavoriteCategories { get; set; }
//        }

//        private async Task LoadAsync(User user)
//        {
//            var userName = await _userManager.GetUserNameAsync(user);
//            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

//            Username = userName;

//            Input = new InputModel
//            {
//                PhoneNumber = phoneNumber
//            };
//        }

//        public async Task<IActionResult> OnGetAsync()
//        {
//            UserProfile = new UserProfileVM
//            {
//                ProfileImage = ProfileImage,
//                ProfileImageUrl = ProfileImageUrl,
//                FavoriteCategories = FavoriteCategories,
//            };

//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//            {
//                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
//            }

//            await LoadAsync(user);
//            return Page();
//        }

//        public async Task<IActionResult> OnPostAsync()
//        {
//            var user = await _userManager.GetUserAsync(User);
//            if (user == null)
//            {
//                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
//            }

//            if (!ModelState.IsValid)
//            {
//                await LoadAsync(user);
//                return Page();
//            }

//            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
//            if (Input.PhoneNumber != phoneNumber)
//            {
//                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
//                if (!setPhoneResult.Succeeded)
//                {
//                    StatusMessage = "Unexpected error when trying to set phone number.";
//                    return RedirectToPage();
//                }
//            }

//            await _signInManager.RefreshSignInAsync(user);
//            StatusMessage = "Your profile has been updated";
//            return RedirectToPage();
//        }
//    }
//}

