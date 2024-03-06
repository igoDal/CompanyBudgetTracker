using CompanyBudgetTracker.Interfaces;
using CompanyBudgetTracker.Models;
using CompanyBudgetTracker.Repositories;


namespace CompanyBudgetTracker.Services;

public class CostIncomeService : ICostIncomeService
{
    private readonly CostIncomeRepository _repository;

    public CostIncomeService(CostIncomeRepository repository)
    {
        _repository = repository;
    }

    public async Task SaveAsync(CostIncomeModel costIncome)
    {

        await _repository.SaveAsync(costIncome);
    }
    
}