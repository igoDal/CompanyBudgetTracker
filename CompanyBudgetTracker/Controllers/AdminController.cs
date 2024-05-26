using System.Linq;
using System.Threading.Tasks;
using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Enums;
using CompanyBudgetTracker.Interfaces;
using CompanyBudgetTracker.Models;
using CompanyBudgetTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyBudgetTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserService _userService;
        private readonly ILogger<AdminController> _logger;
        private readonly MyDbContext _context;

        public AdminController(
            MyDbContext context, 
            ICurrentUserService currentUserService, 
            UserManager<IdentityUser> userManager,
            IUserService userService,
            ILogger<AdminController> logger
        ) : base(context, currentUserService)
        {
            _userManager = userManager;
            _userService = userService;
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = Enum.GetNames(typeof(Roles)).ToList();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = userRoles,
                AllRoles = allRoles,
                SelectedRoles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = model.SelectedRoles.Except(userRoles).ToList();
            var rolesToRemove = userRoles.Except(model.SelectedRoles).ToList();

            if (rolesToAdd.Any())
            {
                result = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }

            if (rolesToRemove.Any())
            {
                result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}