namespace CompanyBudgetTracker.Models;

public class CostIncomeModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Attachment { get; set; }
    

}