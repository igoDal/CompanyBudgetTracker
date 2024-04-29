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

    public async Task<IdentityResult> AssignRoleToUserAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        return await _userManager.AddToRoleAsync(user, roleName);
    }
    
}
