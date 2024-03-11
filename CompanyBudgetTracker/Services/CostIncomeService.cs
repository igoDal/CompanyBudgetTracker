using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Interfaces;
using CompanyBudgetTracker.Models;
using CompanyBudgetTracker.Repositories;


namespace CompanyBudgetTracker.Services;

public class CostIncomeService : ICostIncomeService
{
    private readonly CostIncomeRepository _repository;
    private readonly MyDbContext _context;

    public CostIncomeService(CostIncomeRepository repository, MyDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task SaveAsync(CostIncomeModel costIncome)
    {
        try
        {
            await _repository.SaveAsync(costIncome);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
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
    
}