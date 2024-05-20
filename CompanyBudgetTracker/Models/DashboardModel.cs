namespace CompanyBudgetTracker.Models;

public class DashboardModel
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal ResultIncome { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public Dictionary<string, decimal> IncomeByCategory { get; set; }
    public Dictionary<string, decimal> ExpensesByCategory { get; set; }
    public List<MonthlyDashboard> MonthlyDashboards { get; set; }
}