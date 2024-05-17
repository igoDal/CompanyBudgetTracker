using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Controllers;
using CompanyBudgetTracker.Interfaces;
using CompanyBudgetTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class SettingsController : BaseController
{
    private readonly MyDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<SettingsController> _logger;

    public SettingsController(MyDbContext context, ICurrentUserService currentUserService, ILogger<SettingsController> logger) : base(context, currentUserService)
        
    {
        _context = context;
        _currentUserService = currentUserService;
        _logger = logger;
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
        var currentUserId = _currentUserService.GetUserId();
        model.UserId = currentUserId;

        if (ModelState.ContainsKey("UserId")) {
            ModelState.Remove("UserId");
        }

        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogError($"Validation Error: {error.ErrorMessage}");
            }
            return View(model);
        }

        var userSetting = await _context.UserSettings.FirstOrDefaultAsync(us => us.UserId == currentUserId);
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
        }
        else
        {
            model.UserId = currentUserId;
            _context.UserSettings.Add(model);
        }

        try
        {
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while saving user settings: {ex}");
            ModelState.AddModelError("", "An unexpected error occurred while saving your settings.");
            return View(model);
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