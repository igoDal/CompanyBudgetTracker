using CompanyBudgetTracker.Models;

namespace CompanyBudgetTracker.Interfaces;

public interface ICostIncomeService
{
    Task SaveAsync(CostIncomeModel costIncome);
    Task DeleteAsync(int id);
    Task UpdateSettledStatusAsync(int id, bool settled);

}