using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Interfaces;
using CompanyBudgetTracker.Models;
using CompanyBudgetTracker.Repositories;


namespace CompanyBudgetTracker.Services;

public class CostIncomeService : ICostIncomeService
{
    private readonly CostIncomeRepository _repository;
    private readonly MyDbContext _context;
    private readonly ILogger<CostIncomeService> _logger;

    public CostIncomeService(CostIncomeRepository repository, MyDbContext context, ILogger<CostIncomeService> logger)
    {
        _repository = repository;
        _context = context;
        _logger = logger;
    }

    public async Task SaveAsync(CostIncomeModel costIncome)
    {
        try
        {
            await _repository.SaveAsync(costIncome);

        }
        catch (Exception ex)
        {
            _logger.LogError("ERR: " + ex);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var record = await _context.CostIncomes.FindAsync(id);
        if (record != null)
        {
            _context.CostIncomes.Remove(record);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateSettledStatusAsync(int id, bool settled)
    {
        var record = _context.CostIncomes.FirstOrDefault(x => x.Id == id);
        if (record != null)
        {
            record.Settled = settled;
            await _context.SaveChangesAsync();
        }
    }
    
}