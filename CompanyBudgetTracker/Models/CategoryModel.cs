namespace CompanyBudgetTracker.Models;

public class CategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<CostIncomeModel> Transactions { get; set; }
    public int CategoryId { get; set; }
    public CategoryModel Category { get; set; }
    public ICollection<CostIncomeModelTag> CostIncomeModelTags { get; set; }
}