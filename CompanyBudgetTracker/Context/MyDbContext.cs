using Microsoft.EntityFrameworkCore;

namespace CompanyBudgetTracker.Context;

public class MyDbContext : DbContext

{
    public ApplicationDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<CostIncome> CostIncomes { get; set; }

}