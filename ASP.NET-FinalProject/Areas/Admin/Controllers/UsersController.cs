using ASP.NET_FinalProject.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_FinalProject.Areas.Admin.Controllers
{
    public class UsersController : AdminBaseController
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? user.UserName ?? string.Empty,
                    Roles = roles,
                    IsAdmin = roles.Contains("Administrator")
                });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (!await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                await _userManager.AddToRoleAsync(user, "Administrator");
                TempData["Success"] = $"{user.Email} is now an Administrator.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAdmin(string id)
        {
            if (id == _userManager.GetUserId(User))
            {
                TempData["Error"] = "You cannot remove your own Administrator role.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Administrator");
                TempData["Success"] = $"Administrator role removed from {user.Email}.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}