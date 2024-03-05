using System.Diagnostics;
using CompanyBudgetTracker.Context;
using Microsoft.AspNetCore.Mvc;
using CompanyBudgetTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyBudgetTracker.Controllers;

public class CostIncomeController : Controller
{
    private readonly ILogger<CostIncomeController> _logger;
    private readonly MyDbContext _context;

    public CostIncomeController(ILogger<CostIncomeController> logger, MyDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RedirectToNewRecord()
    {
        return View("NewRecord");
    }

    /*[HttpPost]
    public async Task<IActionResult> SaveTransaction()
    {
        
    }*/
    
}