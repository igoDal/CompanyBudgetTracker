using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Interfaces;
using CompanyBudgetTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyBudgetTracker.Controllers;

[Authorize(Roles = "Admin")]
public class ReportsController : BaseController
{
    private readonly MyDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ReportsController(MyDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
        _context = context;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> IncomeVsExpenseReport(DateTime startDate, DateTime endDate)
    {
        var income = await _context.CostIncomes
            .Where(x => x.Type == "Income" && x.Date >= startDate && x.Date <= endDate)
            .SumAsync(x => x.Amount);

        var expenses = await _context.CostIncomes
            .Where(x => x.Type == "Cost" && x.Date >= startDate && x.Date <= endDate)
            .SumAsync(x => x.Amount);

        var model = new FinancialReportModel
        {
            Income = income,
            Expenses = expenses,
            FinancialResult = income - expenses,
            StartDate = startDate,
            EndDate = endDate
        };

        return View(model);
    }

    
    public async Task<IActionResult> YearlySummaryReport(int year)
    {
        var summary = await _context.CostIncomes
            .Where(x => x.Date.Year == year)
            .GroupBy(x => x.Type)
            .Select(g => new { Type = g.Key, Total = g.Sum(x => x.Amount) })
            .ToListAsync();

        ViewBag.Year = year;
        return View(summary);
    }
}