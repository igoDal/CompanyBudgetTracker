using CompanyBudgetTracker.Enums;
using Microsoft.AspNetCore.Identity;

namespace CompanyBudgetTracker.Data.Initializers;

public class RoleInitializer
{
    private readonly UserManager<IdentityUser> _userManager;

    public RoleInitializer(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var role in Enum.GetNames(typeof(Roles)))
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
    
    public async Task AssignRoleToUser(string userId, Roles role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found.");
        }

        string roleName = Enum.GetName(typeof(Roles), role);
        if (!await _userManager.IsInRoleAsync(user, roleName))
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
    }

}