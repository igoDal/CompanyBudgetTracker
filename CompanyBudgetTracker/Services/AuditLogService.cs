using CompanyBudgetTracker.Context;
using CompanyBudgetTracker.Interfaces;
using CompanyBudgetTracker.Models;

namespace CompanyBudgetTracker.Services;

public class AuditLogService : IAuditLogService
{
    private readonly MyDbContext _context;

    public AuditLogService(MyDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(string action, string userId, string userName, string details)
    {
        var auditLog = new AuditLog
        {
            Action = action,
            UserId = userId,
            UserName = userName,
            Timestamp = DateTime.UtcNow,
            Details = details
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }
}