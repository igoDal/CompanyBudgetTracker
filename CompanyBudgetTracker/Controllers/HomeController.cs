using System.Diagnostics;
using CompanyBudgetTracker.Context;
using Microsoft.AspNetCore.Mvc;
using CompanyBudgetTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyBudgetTracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MyDbContext _context;

    public HomeController(ILogger<HomeController> logger, MyDbContext context)
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
                .Where(x => x.Type == "Income")
                .SumAsync(x => x.Amount),
            TotalExpenses = await _context.CostIncomes
                .Where(x => x.Type == "Cost")
                .SumAsync(x => x.Amount)
        };

        viewModel.ResultIncome = viewModel.TotalIncome - viewModel.TotalExpenses;

        return View(viewModel);
    }
}