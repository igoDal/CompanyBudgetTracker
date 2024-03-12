using System.Diagnostics;
using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CompanyBudgetTracker.Models;
using CompanyBudgetTracker.Services;
using Microsoft.EntityFrameworkCore;

namespace CompanyBudgetTracker.Controllers;

public class CostIncomeController : Controller
{
    private readonly ILogger<CostIncomeController> _logger;
    private readonly MyDbContext _context;
    private readonly ICostIncomeService _costIncomeService;

    public CostIncomeController(
        ILogger<CostIncomeController> logger, 
        MyDbContext context,
        ICostIncomeService costIncomeService)
    {
        _logger = logger;
        _context = context;
        _costIncomeService = costIncomeService;

    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RedirectToNewRecord()
    {
        return View("NewRecord");
    }

    public IActionResult GetHistory()
    {
        var records = _context.CostIncomes.ToList();

        return View("History", records);
    }
    
    [HttpPost]
    public async Task<IActionResult> SaveTransaction(CostIncomeModel model, IFormFile transactionAtt)
    {
        if (transactionAtt != null && transactionAtt.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await transactionAtt.CopyToAsync(memoryStream);
                model.Attachment = memoryStream.ToArray(); 
            }
            model.AttachmentName = transactionAtt.FileName;
            model.AttachmentContentType = transactionAtt.ContentType;
        }
        
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach(var error in errors)
                {
                    Console.WriteLine(error);
                }
            }
            else if (ModelState.IsValid)
            {
                await _costIncomeService.SaveAsync(model);
                return RedirectToAction("Index");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("ERR: " + ex);
        }

        return View("Index");
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        try
        {
            await _costIncomeService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting transaction with ID {id}: {ex}");
            return View("Error"); 
        }
    }
    
}