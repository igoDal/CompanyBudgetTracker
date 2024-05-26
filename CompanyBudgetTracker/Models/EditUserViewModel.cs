namespace CompanyBudgetTracker.Models;

public class EditUserViewModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    
    public IList<string> Roles { get; set; } = new List<string>();
    public IList<string> AllRoles { get; set; } = new List<string>();
    public IList<string> SelectedRoles { get; set; } = new List<string>();
}