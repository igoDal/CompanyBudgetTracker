using CompanyBudgetTracker.Enums;
using Microsoft.AspNetCore.Identity;

namespace CompanyBudgetTracker.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> AssignRoleToUserAsync(string userId, Roles role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        string roleName = Enum.GetName(typeof(Roles), role);
        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    }

}
