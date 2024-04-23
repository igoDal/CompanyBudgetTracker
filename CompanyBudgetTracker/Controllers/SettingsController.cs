using CompanyBudgetTracker.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class SettingsController : Controller
{
    private readonly MyDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public SettingsController(MyDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var userSetting = await _context.UserSettings
            .FirstOrDefaultAsync(us => us.UserId == userId);

        if (userSetting == null)
        {
            userSetting = new UserSettings { UserId = userId, EnableNotifications = true };
            _context.UserSettings.Add(userSetting);
            await _context.SaveChangesAsync();
        }

        return View("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserSettings userSetting)
    {
        if (ModelState.IsValid)
        {
            _context.Update(userSetting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View("Index");
    }
}