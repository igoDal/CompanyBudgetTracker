using CompanyBudgetTracker.Enums;

namespace CompanyBudgetTracker.Models;

public class AlertSetting
{
    public int Id { get; set; }
    public ThresholdType ThresholdType { get; set; }
    public decimal ThresholdAmount { get; set; }
    public bool NotifyByEmail { get; set; }
    public bool NotifyInApp { get; set; }
}

