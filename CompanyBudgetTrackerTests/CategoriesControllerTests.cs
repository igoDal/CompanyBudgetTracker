using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Controllers;
using CompanyBudgetTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CategoriesControllerTests
{
    private readonly MyDbContext _context;
    private readonly CategoriesController _controller;

    public CategoriesControllerTests()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new MyDbContext(options);
        _controller = new CategoriesController(_context);
    }

    [Fact]
    public async Task Index_ReturnsViewWithCategories()
    {
        // Arrange
        _context.Categories.Add(new CategoryModel { Id = 1, Name = "Category1" });
        _context.Categories.Add(new CategoryModel { Id = 2, Name = "Category2" });
        _context.SaveChanges();

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<CategoryModel>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }
}