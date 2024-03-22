namespace CompanyBudgetTracker.Models;

public class TagModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<CostIncomeModelTag> CostIncomeModelTags { get; set; }
}