using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CompanyBudgetTracker.Models;

namespace CompanyBudgetTracker.Controllers;

public class CostIncomeController : Controller
{
    private readonly ILogger<CostIncomeController> _logger;

    public CostIncomeController(ILogger<CostIncomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RedirectToNewRecord()
    {
        return View("NewRecord");
    }
    
}