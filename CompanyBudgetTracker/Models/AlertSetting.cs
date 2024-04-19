namespace CompanyBudgetTracker.Models;

public class AlertSetting
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public decimal ThresholdAmount { get; set; }
    public string ThresholdType { get; set; } 
    public bool NotifyByEmail { get; set; }
    public bool NotifyInApp { get; set; }
}
