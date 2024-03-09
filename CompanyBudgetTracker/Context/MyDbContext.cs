using CompanyBudgetTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyBudgetTracker.Context;

public class MyDbContext : DbContext

{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<CostIncomeModel> CostIncomes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CostIncomeModel>().ToTable("CostIncome")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();;
    }

}