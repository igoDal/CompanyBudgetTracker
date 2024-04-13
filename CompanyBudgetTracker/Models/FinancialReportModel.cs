namespace CompanyBudgetTracker.Models;

public class FinancialReportModel
{
    public decimal Income { get; set; }
    public decimal Expenses { get; set; }
    public decimal FinancialResult { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
