using System.ComponentModel.DataAnnotations;

namespace CompanyBudgetTracker.Models;

public class CostIncomeModel
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Attachment { get; set; }
    

}