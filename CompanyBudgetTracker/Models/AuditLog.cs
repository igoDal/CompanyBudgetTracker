namespace CompanyBudgetTracker.Models;

public class AuditLog
{
    public int Id { get; set; }
    public string Action { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public DateTime Timestamp { get; set; }
    public string Details { get; set; }
}
