using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Interfaces;
using CompanyBudgetTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class SettingsController : Controller
{
    private readonly MyDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public SettingsController(MyDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<IActionResult> Index()
    {
        var userSetting = await GetUserSettings();
        
        return View(userSetting);
    }

    
    public async Task<IActionResult> EditUserSettings()
    {
        var userSetting = await GetUserSettings();
        return View(userSetting);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUserSettings(UserSettings model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userSetting = await _context.UserSettings.FindAsync(model.Id);
        if (userSetting != null)
        {
            _context.Entry(userSetting).CurrentValues.SetValues(model);
        }
        else
        {
            _context.UserSettings.Add(model);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
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
                _context.Entry(existingSetting).CurrentValues.SetValues(model);
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

    private async Task<UserSettings> GetUserSettings()
    {
        var userId = _currentUserService.GetUserId();
        var userSetting = await _context.UserSettings.FirstOrDefaultAsync(us => us.UserId == userId);
        if (userSetting == null)
        {
            userSetting = GetDefaultUserSettings(userId);
            _context.UserSettings.Add(userSetting);
            await _context.SaveChangesAsync();
        }

        return userSetting;
    }
    private UserSettings GetDefaultUserSettings(string userId)
    {
        return new UserSettings
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
    
    
}