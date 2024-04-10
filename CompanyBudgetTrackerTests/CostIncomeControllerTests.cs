using Xunit;
using Moq;
using CompanyBudgetTracker.Context;
using Microsoft.EntityFrameworkCore;
using CompanyBudgetTracker.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using CompanyBudgetTracker.Interfaces;
using CompanyBudgetTracker.Models;

public class CostIncomeControllerTests
{
    private readonly Mock<MyDbContext> _mockContext;
    private readonly Mock<ICostIncomeService> _mockService;
    private readonly CostIncomeController _controller;

    public CostIncomeControllerTests()
    {
        _mockContext = new Mock<MyDbContext>(new DbContextOptions<MyDbContext>());
        _mockService = new Mock<ICostIncomeService>();
        _controller = new CostIncomeController(null, _mockContext.Object, _mockService.Object);
    }
    
    
    [Fact]
    public async Task DeleteTransaction_CallsServiceAndRedirects()
    {
        // Arrange
        var transactionId = 1;
        _mockService.Setup(service => service.DeleteAsync(transactionId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteTransaction(transactionId);

        // Assert
        _mockService.Verify(service => service.DeleteAsync(transactionId), Times.Once);
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }
    
    
    //Test for specific record history (filtered)
    [Fact]
    public async Task GetHistory_WithParameters_ReturnsCorrectViewAndModel()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        
        using (var context = new MyDbContext(options))
        {
            context.CostIncomes.AddRange(
                new CostIncomeModel { 
                    Id = 1, 
                    Name = "TestItem1", 
                    Type = "Income", 
                    Amount = 100, 
                    Date = DateTime.Now, 
                    Attachment = new byte[] { 0x03, 0x04 }, 
                    CategoryId = 1,
                    CategoryName = "Test"
                },
                new CostIncomeModel { 
                    Id = 2, 
                    Name = "TestItem2", 
                    Type = "Cost", 
                    Amount = 50, 
                    Date = DateTime.Now, 
                    Attachment = new byte[] { 0x03, 0x04 }, 
                    CategoryId = 1,
                    CategoryName = "Test" }
            );
            context.SaveChanges();
        }
        
        using (var context = new MyDbContext(options))
        {
            var controller = new CostIncomeController(null, context, null);
            
            string itemName = "TestItem1";
            int? itemId = 1;
            string itemType = "Income";
            DateTime? startDate = DateTime.Now.AddDays(-1);
            DateTime? endDate = DateTime.Now.AddDays(1);
            string sortOrder = "name_asc";
            int page = 1;
            int pageSize = 10;

            // Act
            var result = controller.GetHistory(itemName, itemId, itemType, startDate, endDate, sortOrder, page, pageSize);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<CostIncomeModel>>(viewResult.Model);
            Assert.Single(model);
        }
    }



}