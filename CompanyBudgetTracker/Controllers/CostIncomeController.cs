using System.Diagnostics;
using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CompanyBudgetTracker.Models;
using CompanyBudgetTracker.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        var categories = _context.Categories
            .Select(c => new { c.Id, c.Name })
            .ToList();

        ViewData["Categories"] = new SelectList(categories, "Id", "Name");

        return View("NewRecord");
    }

    public IActionResult GetHistory
        (string? itemName, int? itemId, string? itemType, DateTime? startDate, DateTime? endDate,  string? sortOrder, int page = 1, int pageSize = 10)
    {
        ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
        ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
        ViewBag.TypeSortParm = sortOrder == "type_asc" ? "type_desc" : "type_asc";
        ViewBag.AmountSortParm = sortOrder == "amount_asc" ? "amount_desc" : "amount_asc";
        ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
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
        
        var totalAmount = query.Sum(x => x.Amount);
        
        switch (sortOrder)
        {
            case "date_asc":
                query = query.OrderBy(x => x.Date);
                break;
            case "amount_desc":
                query = query.OrderByDescending(x => x.Amount);
                break;
            case "name_desc":
                query = query.OrderByDescending(x => x.Name);
                break;
            case "name_asc":
                query = query.OrderBy(x => x.Name);
                break;
            default:
                query = query.OrderByDescending(x => x.Id);
                break;
        }
        
        int totalRecords = query.Count();
        var records = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        
        ViewData["TotalRecords"] = totalRecords;
        ViewData["CurrentPage"] = page;
        ViewData["PageSize"] = pageSize;
        
        int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        ViewData["TotalPages"] = totalPages;

        return View("History", records);
        
        /*ViewData["TotalAmount"] = totalAmount;
        var records = query.ToList();
        return View("History", records);*/
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

            await _costIncomeService.SaveAsync(model);
            return RedirectToAction("Index");
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

        return View("Index");
    }

    public async Task<IActionResult> DeleteRecord(int itemId)
    {
        if (itemId == null)
        {
            return NotFound();
        }

        await _costIncomeService.DeleteAsync(itemId);

        return View("Index");
    }
    
    public async Task<IActionResult> Details(int? itemId)
    {
        if (itemId == null)
        {
            return NotFound();
        }

        var record = await _context.CostIncomes
            .FirstOrDefaultAsync(m => m.Id == itemId);
        if (record == null)
        {
            return NotFound();
        }

        return View("RecordDetails", record);
    }

    
    
}