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

    public async Task<IActionResult> Dashboard()
    {
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