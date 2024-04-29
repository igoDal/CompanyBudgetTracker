using CompanyBudgetTracker.Enums;
using Microsoft.AspNetCore.Identity;

namespace CompanyBudgetTracker.Services;

public interface IUserService
{
    Task<IdentityResult> AssignRoleToUserAsync(string userId, string roleName);
}