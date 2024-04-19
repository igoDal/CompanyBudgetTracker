using CompanyBudgetTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyBudgetTracker.Context;

public class MyDbContext : IdentityDbContext<IdentityUser>

{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<CategoryModel> Categories { get; set; }
    public DbSet<CostIncomeModel> CostIncomes { get; set; }
    public DbSet<AssetModel> Assets { get; set; }
    public DbSet<LiabilityModel> Liabilities { get; set; }
    public DbSet<AlertSetting> AlertSettings { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AssetModel>().HasKey(a => a.AssetId);
        modelBuilder.Entity<LiabilityModel>().HasKey(a => a.LiabilityId);
        
        modelBuilder.Entity<AssetModel>().ToTable("Assets");
        modelBuilder.Entity<LiabilityModel>().ToTable("Liabilities");
        modelBuilder.Entity<CostIncomeModel>().ToTable("CostIncome")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
    }

}