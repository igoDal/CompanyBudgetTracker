using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompanyBudgetTracker.Controllers;
using CompanyBudgetTracker.Models;
using CompanyBudgetTracker.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;

public class HomeControllerTests
{
    private readonly Mock<ILogger<HomeController>> _mockLogger;
    private readonly MyDbContext _context;
    
    public HomeControllerTests()
    {
        _mockLogger = new Mock<ILogger<HomeController>>();

        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;

        _context = new MyDbContext(options);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public void Index_ReturnsAViewResult()
    {
        // Arrange
        var controller = new HomeController(_mockLogger.Object, _context);

        // Act
        var result = controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error_ReturnsAViewResult_WithErrorViewModel()
    {
        // Arrange
        var controller = new HomeController(_mockLogger.Object, _context);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Act
        var result = controller.Error() as ViewResult;
        
        // Assert
        Assert.NotNull(result);
        var model = Assert.IsType<ErrorViewModel>(result.Model);
        Assert.NotNull(model.RequestId);
    }
    
    [Fact]
    public async Task Dashboard_ReturnsAViewResult_WithDashboardModel()
    {
        // Arrange
        var controller = new HomeController(_mockLogger.Object, _context);
        
        _context.CostIncomes.Add(new CostIncomeModel { Type = "Income", Amount = 1000m, Date = DateTime.Today, Attachment = new byte[] { 0x03, 0x04 }, 
            CategoryId = 1,
            CategoryName = "Test", Name = "Test" });
        _context.CostIncomes.Add(new CostIncomeModel { Type = "Cost", Amount = 500m, Date = DateTime.Today, Attachment = new byte[] { 0x03, 0x04 }, 
            CategoryId = 1,
            CategoryName = "Test", Name = "Test" });
        await _context.SaveChangesAsync();

        // Act
        var result = await controller.Dashboard(null, null, "last30days") as ViewResult;
        
        // Assert
        Assert.NotNull(result);
        var model = Assert.IsType<DashboardModel>(result.Model);
        Assert.Equal(1000m, model.TotalIncome);
        Assert.Equal(500m, model.TotalExpenses);
        Assert.Equal(500m, model.ResultIncome);
    }

}
