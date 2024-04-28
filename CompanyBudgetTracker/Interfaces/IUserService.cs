using CompanyBudgetTracker.Enums;

namespace CompanyBudgetTracker.Services;

public interface IUserService
{
    Task<bool> AssignRoleToUserAsync(string userId, Roles role);
}