using Microsoft.AspNetCore.Mvc;
using CompanyBudgetTracker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using CompanyBudgetTracker.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CompanyBudgetTracker.Controllers;

public class CategoriesController : Controller
{
    private readonly MyDbContext _context;

    public CategoriesController(MyDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories.ToListAsync();
        return View(categories);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(CategoryModel category)
    {
        if (ModelState.IsValid)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View("Index");
    }
}