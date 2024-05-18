using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsTella.Models.Database;

namespace NewsTella.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(
            RoleManager<IdentityRole> roleManager
            )
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateRoles()
        {
            var adminExists = await _roleManager.RoleExistsAsync(Roles.Admin.ToString());
            if (!adminExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }

            var editorExists = await _roleManager.RoleExistsAsync(Roles.Editor.ToString());
            if (!adminExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Editor.ToString()));
            }

            var writerExists = await _roleManager.RoleExistsAsync(Roles.Writer.ToString());
            if (!adminExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Writer.ToString()));
            }

            var memberExists = await _roleManager.RoleExistsAsync(Roles.Member.ToString());
            if (!adminExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Member.ToString()));

            }

            return RedirectToAction(nameof(Index));
        }
    }
}
