namespace CompanyBudgetTracker.Models;

public class LiabilityModel
{
    public int LiabilityId { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public bool IsCurrent { get; set; }
    public DateTime DueDate { get; set; }
}