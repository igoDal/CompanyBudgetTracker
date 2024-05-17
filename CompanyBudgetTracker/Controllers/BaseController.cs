using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyBudgetTracker.Controllers;

public class BaseController : Controller
{
    private readonly MyDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public BaseController(MyDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var currentUserId = _currentUserService.GetUserId();
        var userSettings = _context.UserSettings.FirstOrDefault(us => us.UserId == currentUserId);
        var themeClass = userSettings?.Theme == "Dark" ? "dark-mode" : "light-mode";
        ViewData["ThemeClass"] = themeClass;

        base.OnActionExecuting(context);
    }
}
