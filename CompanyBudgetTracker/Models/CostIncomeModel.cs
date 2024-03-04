namespace CompanyBudgetTracker.Models;

public class CostIncomeModel
{
    public int Id { get; set; }
    public decimal Cost { get; set; }
    public decimal Income { get; set; }
    public DateTime Date { get; set; }

}