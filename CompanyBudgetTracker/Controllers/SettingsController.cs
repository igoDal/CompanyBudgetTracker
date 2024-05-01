using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Models;
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
            var dbEntry = await _context.UserSettings.FindAsync(userSetting.Id);
            if (dbEntry != null)
            {
                dbEntry.EnableNotifications = userSetting.EnableNotifications;
                dbEntry.Theme = userSetting.Theme; 
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _context.UserSettings.Add(userSetting);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
        return View("EditUserSettings");
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveAlertSettings(AlertSetting model)
    {
        if (ModelState.IsValid)
        {
            var existingSetting = await _context.AlertSettings.FindAsync(model.Id);
            if (existingSetting != null)
            {
                
                existingSetting.ThresholdType = model.ThresholdType;
                existingSetting.ThresholdAmount = model.ThresholdAmount;
                existingSetting.NotifyByEmail = model.NotifyByEmail;
                existingSetting.NotifyInApp = model.NotifyInApp;
            }
            else
            {
                _context.AlertSettings.Add(model);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View("Index");
    }
    
    
}