namespace CompanyBudgetTracker.Models;

public class UserDetailsViewModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public IList<string> Roles { get; set; }
    public bool IsActive { get; set; }
}
