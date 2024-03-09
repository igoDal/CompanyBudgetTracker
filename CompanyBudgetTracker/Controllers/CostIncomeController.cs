using System.Diagnostics;
using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CompanyBudgetTracker.Models;
using CompanyBudgetTracker.Services;
using Microsoft.EntityFrameworkCore;

namespace CompanyBudgetTracker.Controllers;

public class CostIncomeController : Controller
{
    private readonly ILogger<CostIncomeController> _logger;
    private readonly MyDbContext _context;
    private readonly ICostIncomeService _costIncomeService;

    public CostIncomeController(
        ILogger<CostIncomeController> logger, 
        MyDbContext context,
        ICostIncomeService costIncomeService)
    {
        _logger = logger;
        _context = context;
        _costIncomeService = costIncomeService;

    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RedirectToNewRecord()
    {
        return View("NewRecord");
    }

    public IActionResult GetHistory()
    {
        var records = _context.CostIncomes.ToList();

        return View("History", records);
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveTransaction([FromBody] CostIncomeModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _costIncomeService.SaveAsync(model);
                return RedirectToAction("Index");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return View("Index");
    }
    
}