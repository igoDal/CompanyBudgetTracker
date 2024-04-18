namespace CompanyBudgetTracker.Models;

public class UserSettings
{
    public int UserSettingId { get; set; }
    public string UserId { get; set; } 
    public string PreferredCurrency { get; set; }
    public DateTime FinancialYearStart { get; set; }
    public virtual ICollection<CategoryModel> CustomCategories { get; set; }
}