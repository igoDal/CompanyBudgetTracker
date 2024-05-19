using System.Diagnostics;
using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CompanyBudgetTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyBudgetTracker.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly MyDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public HomeController(ILogger<HomeController> logger, MyDbContext context, ICurrentUserService currentUserService) : base (context, currentUserService)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> Dashboard(DateTime? startDate, DateTime? endDate, string range = "")
    {
        DateTime calculatedStartDate, calculatedEndDate;

        switch (range.ToLower())
        {
            case "last30days":
                calculatedStartDate = DateTime.Today.AddDays(-30);
                calculatedEndDate = DateTime.Today;
                break;
            case "last3months":
                calculatedStartDate = DateTime.Today.AddMonths(-3);
                calculatedEndDate = DateTime.Today;
                break;
            case "lastyear":
                calculatedStartDate = DateTime.Today.AddYears(-1);
                calculatedEndDate = DateTime.Today;
                break;
            default:
                calculatedStartDate = startDate ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                calculatedEndDate = endDate ?? DateTime.Today;
                break;
        }

        var viewModel = new DashboardModel
        {
            TotalIncome = await _context.CostIncomes
                .Where(x => x.Type == "Income" && x.Date >= calculatedStartDate && x.Date <= calculatedEndDate)
                .SumAsync(x => x.Amount),
            TotalExpenses = await _context.CostIncomes
                .Where(x => x.Type == "Cost" && x.Date >= calculatedStartDate && x.Date <= calculatedEndDate)
                .SumAsync(x => x.Amount),
            StartDate = calculatedStartDate,
            EndDate = calculatedEndDate
        };

        viewModel.ResultIncome = viewModel.TotalIncome - viewModel.TotalExpenses;

        return View(viewModel);
    }


    
    public async Task<IActionResult> FinancialHealth()
    {
        var today = DateTime.Today;
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        
        var totalIncomeThisMonth = await _context.CostIncomes
            .Where(x => x.Date >= startOfMonth && x.Type == "Income")
            .SumAsync(x => x.Amount);

        var totalExpensesThisMonth = await _context.CostIncomes
            .Where(x => x.Date >= startOfMonth && x.Type == "Cost")
            .SumAsync(x => x.Amount);

        var totalLiabilities = await _context.Liabilities.SumAsync(x => x.Amount);

        var financialHealthViewModel = new FinancialHealthModel
        {
            CashFlow = totalIncomeThisMonth - totalExpensesThisMonth,
            DebtToIncomeRatio = totalIncomeThisMonth != 0 ? (totalLiabilities / totalIncomeThisMonth) : 0,
            QuickRatio = await CalculateQuickRatio()
        };

        return View("FinancialHealth");
    }

    private async Task<decimal> CalculateQuickRatio()
    {
        var liquidAssets = await _context.Assets.Where(x => x.IsLiquid).SumAsync(x => x.Value);
        var currentLiabilities = await _context.Liabilities.Where(x => x.IsCurrent).SumAsync(x => x.Amount);
    
        return currentLiabilities != 0 ? (liquidAssets / currentLiabilities) : 0;
    }

}