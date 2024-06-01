using CompanyBudgetTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyBudgetTracker.Context;

public class MyDbContext : IdentityDbContext<ApplicationUser>

{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<CategoryModel> Categories { get; set; }
    public DbSet<CostIncomeModel> CostIncomes { get; set; }
    public DbSet<AssetModel> Assets { get; set; }
    public DbSet<LiabilityModel> Liabilities { get; set; }
    public DbSet<AlertSetting> AlertSettings { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }
    


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AssetModel>().HasKey(a => a.AssetId);
        modelBuilder.Entity<LiabilityModel>().HasKey(a => a.LiabilityId);
        
        modelBuilder.Entity<AlertSetting>()
            .Property(p => p.ThresholdAmount)
            .HasColumnType("decimal(20, 6)");
        modelBuilder.Entity<AssetModel>()
            .Property(p => p.Value)
            .HasColumnType("decimal(20, 6)");
        
        modelBuilder.Entity<CostIncomeModel>()
            .Property(p => p.Amount)
            .HasColumnType("decimal(20, 6)");
        
        modelBuilder.Entity<LiabilityModel>()
            .Property(p => p.Amount)
            .HasColumnType("decimal(20, 6)");
        
        modelBuilder.Entity<AssetModel>().ToTable("Assets");
        modelBuilder.Entity<LiabilityModel>().ToTable("Liabilities");
        modelBuilder.Entity<CostIncomeModel>().ToTable("CostIncome")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<UserSettings>()
            .HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(s => s.UserId);
    }

}