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

}