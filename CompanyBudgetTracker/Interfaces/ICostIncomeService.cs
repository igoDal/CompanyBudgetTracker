using CompanyBudgetTracker.Models;

namespace CompanyBudgetTracker.Interfaces;

public interface ICostIncomeService
{
    public Task SaveAsync(CostIncomeModel costIncome);
}