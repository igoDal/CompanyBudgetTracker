using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyBudgetTracker.Repositories;

public class CostIncomeRepository
{
    private readonly MyDbContext _context;

    public CostIncomeRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(CostIncomeModel costIncome)
    {
        if (costIncome.Id == 0)
        {
            _context.Add(costIncome);
        }
        else
        {
            _context.Update(costIncome);
        }

        await _context.SaveChangesAsync();
    }
}