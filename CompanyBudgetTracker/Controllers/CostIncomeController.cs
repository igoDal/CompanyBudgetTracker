using System.Diagnostics;
using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CompanyBudgetTracker.Models;
using CompanyBudgetTracker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

    public IActionResult GetHistory(string? itemName, int? itemId, string? itemType, DateTime? startDate, DateTime? endDate)
    {
        var query = _context.CostIncomes.AsQueryable();
        
        if (itemId.HasValue)
            query = query.Where(x => x.Id == itemId.Value);
        if (!string.IsNullOrWhiteSpace(itemName))
            query = query.Where(x => x.Name.Contains(itemName));
        if (!string.IsNullOrWhiteSpace(itemType))
            query = query.Where(x => x.Type.Contains(itemType));
        if (startDate.HasValue)
            query = query.Where(x => x.Date >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(x => x.Date <= endDate.Value);
        
        
        /*var records = _context.CostIncomes
            .Where(record => 
                (name.IsNullOrEmpty() || record.Name.Contains(name)) &&
                (!id.HasValue || record.Id == id) &&
                (type.IsNullOrEmpty() || record.Name.Contains(type)) &&
                (!startDate.HasValue || record.Date >= startDate) && 
                (!endDate.HasValue || record.Date <= endDate))
            .ToList();*/
        
        var totalAmount = query.Sum(x => x.Amount);
        
        ViewData["TotalAmount"] = totalAmount;
        var records = query.ToList();
        return View("History", records);

        //return View("History", records);
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
    
    public IActionResult OpenAttachment(int attachmentId)
    {
        var attachment = _context.CostIncomes.FirstOrDefault(x => x.Id == attachmentId);
        if (attachment == null)
        {
            return NotFound();
        }

        return File(attachment.Attachment, attachment.AttachmentContentType, attachment.AttachmentName);
    }

    public async Task<IActionResult> Settle(int itemId, bool settled)
    {
        var record = _context.CostIncomes.FirstOrDefault(x => x.Id == itemId);

        if (itemId == null)
        {
            return NotFound();
        }

        await _costIncomeService.UpdateSettledStatusAsync(itemId, settled);

        return View("History");
    }
}