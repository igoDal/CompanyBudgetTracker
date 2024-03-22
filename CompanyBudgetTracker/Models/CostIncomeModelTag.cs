namespace CompanyBudgetTracker.Models;

public class CostIncomeModelTag
{
    public int CostIncomeModelId { get; set; }
    public CostIncomeModel CostIncomeModel { get; set; }
    public int TagId { get; set; }
    public TagModel TagModel { get; set; }
}