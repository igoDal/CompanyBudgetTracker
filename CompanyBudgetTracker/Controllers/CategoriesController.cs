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

    
    public IActionResult Add()
    {
        return View(new CategoryModel());
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(CategoryModel category)
    {
        bool categoryExists = _context.Categories.Any(c => c.Name.ToLower() == category.Name.ToLower());
        if (categoryExists)
        {
            ModelState.AddModelError("Name", "A category with the same name already exists.");
            return View(category);
        }

        if (ModelState.IsValid)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }
    
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryModel category)
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.Id == id);
    }
    
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories
            .FirstOrDefaultAsync(m => m.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        try
        {

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return Ok();
    }

}