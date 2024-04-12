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
    
    [Fact]
    public async Task Add_Post_ValidCategory_AddsCategoryAndRedirects()
    {
        // Arrange
        var category = new CategoryModel { Name = "New Category" };

        // Act
        var result = await _controller.Add(category);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        var addedCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "New Category");
        Assert.NotNull(addedCategory);
    }

    [Fact]
    public async Task Edit_Post_ValidData_UpdatesCategoryAndRedirects()
    {
        // Arrange
        var category = new CategoryModel { Id = 1, Name = "Original Category" };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        
        var categoryToUpdate = await _context.Categories.FindAsync(1);
        categoryToUpdate.Name = "Updated Category";

        // Act
        var result = await _controller.Edit(1, categoryToUpdate);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        var editedCategory = await _context.Categories.FindAsync(1);
        Assert.Equal("Updated Category", editedCategory.Name);
    }

    
}