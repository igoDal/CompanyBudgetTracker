namespace CompanyBudgetTracker.Interfaces;

public interface IAuditLogService
{
    Task LogAsync(string action, string userId, string userName, string details);
}