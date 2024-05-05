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
        var userSetting = await _context.UserSettings.FirstOrDefaultAsync(us => us.UserId == userId);

        if (userSetting == null)
        {
            userSetting = new UserSettings
            {
                UserId = userId,
                EnableNotifications = true,
                NotifyByEmail = true,
                NotifyBySMS = false,
                NotifyInApp = true,
                NotifyOnNewMessage = true,
                NotifyOnTaskCompletion = true,
                NotifyOnDueDateApproach = true,
                Language = "en-US",
                Theme = "Light"
            };
        }

        return View(userSetting);
    }

    
    public async Task<IActionResult> EditUserSettings()
    {
        var userId = _userManager.GetUserId(User);
        var userSetting = await _context.UserSettings.FirstOrDefaultAsync(us => us.UserId == userId);

        if (userSetting == null)
        {
            userSetting = new UserSettings
            {
                UserId = userId,
                EnableNotifications = true,
                NotifyByEmail = true,
                NotifyBySMS = false,
                NotifyInApp = true,
                NotifyOnNewMessage = true,
                NotifyOnTaskCompletion = true,
                NotifyOnDueDateApproach = true,
                Language = "en-US",
                Theme = "Light"
            };
        }

        
        return View(userSetting);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUserSettings(UserSettings model)
    {
        /*if (!ModelState.IsValid)
        {
            return View(model);
        }*/

        var userSetting = await _context.UserSettings.FindAsync(model.Id);
        if (userSetting != null)
        {
            userSetting.EnableNotifications = model.EnableNotifications;
            userSetting.NotifyByEmail = model.NotifyByEmail;
            userSetting.NotifyBySMS = model.NotifyBySMS;
            userSetting.NotifyInApp = model.NotifyInApp;
            userSetting.NotifyOnNewMessage = model.NotifyOnNewMessage;
            userSetting.NotifyOnTaskCompletion = model.NotifyOnTaskCompletion;
            userSetting.NotifyOnDueDateApproach = model.NotifyOnDueDateApproach;
            userSetting.Language = model.Language;
            userSetting.Theme = model.Theme;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        else
        {
            _context.UserSettings.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
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