using Microsoft.AspNetCore.Identity;
namespace CompanyBudgetTracker.Models;

public class ApplicationUser : IdentityUser
{
    public bool IsActive { get; set; } = true; 
    
}
