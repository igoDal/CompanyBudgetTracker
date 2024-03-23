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
    public DbSet<TagModel> Tags { get; set; }
    public DbSet<CostIncomeModel> CostIncomes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CostIncomeModel>().ToTable("CostIncome")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<CostIncomeModelTag>()
            .HasKey(t => new { t.CostIncomeModelId, t.TagId });

        modelBuilder.Entity<CostIncomeModelTag>()
            .HasOne(pt => pt.CostIncomeModel)
            .WithMany(p => p.CostIncomeModelTags)
            .HasForeignKey(pt => pt.CostIncomeModelId);

        modelBuilder.Entity<CostIncomeModelTag>()
            .HasOne(pt => pt.TagModel)
            .WithMany(t => t.CostIncomeModelTags)
            .HasForeignKey(pt => pt.TagId);
    }

}