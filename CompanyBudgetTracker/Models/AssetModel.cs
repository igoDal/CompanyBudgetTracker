namespace CompanyBudgetTracker.Models;

public class AssetModel
{
    public int AssetId { get; set; }
    public string Name { get; set; }
    public decimal Value { get; set; }
    public bool IsLiquid { get; set; }
    public DateTime DateAcquired { get; set; }
}