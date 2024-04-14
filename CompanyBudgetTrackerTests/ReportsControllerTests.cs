using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Threading.Tasks;
using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Controllers;
using CompanyBudgetTracker.Models;
using Microsoft.AspNetCore.Mvc;

public class ReportsControllerTests
{
    private readonly MyDbContext _context;
    private readonly ReportsController _controller;

    public ReportsControllerTests()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestReportsDb")
            .Options;

        _context = new MyDbContext(options);
        _controller = new ReportsController(_context);
    }

    [Fact]
    public async Task IncomeVsExpenseReport_ReturnsCorrectData()
    {
        // Arrange
        _context.CostIncomes.AddRange(
            new CostIncomeModel { Type = "Income", Amount = 1000, Date = new DateTime(2023, 1, 15), Attachment = new byte[]{0x3, 0x2}, CategoryName = "Test1", Name = "Test"},
            new CostIncomeModel { Type = "Income", Amount = 500, Date = new DateTime(2023, 1, 20), Attachment = new byte[]{0x3, 0x2}, CategoryName = "Test1", Name = "Test" },
            new CostIncomeModel { Type = "Cost", Amount = 300, Date = new DateTime(2023, 1, 15), Attachment = new byte[]{0x3, 0x2}, CategoryName = "Test1", Name = "Test" },
            new CostIncomeModel { Type = "Cost", Amount = 200, Date = new DateTime(2023, 1, 25), Attachment = new byte[]{0x3, 0x2}, CategoryName = "Test1", Name = "Test" }
        );
        await _context.SaveChangesAsync();

        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 1, 31);

        // Act
        var result = await _controller.IncomeVsExpenseReport(startDate, endDate);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        dynamic model = viewResult.Model;
        Assert.Equal(1500, model.Income);
        Assert.Equal(500, model.Expenses);
        Assert.Equal(1000, model.FinancialResult);
    }
}