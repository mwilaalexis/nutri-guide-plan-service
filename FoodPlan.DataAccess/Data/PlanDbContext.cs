using Microsoft.EntityFrameworkCore;
using FoodPlan.DataAccess.Entities;
namespace FoodPlan.DataAccess.Data;
public class PlanDbContext : DbContext
{
    public DbSet<MealPlan> MealPlans { get; set; }
    public DbSet<DailyMeal> DailyMeals { get; set; }
    public DbSet<Meal> Meals { get; set; }

    public PlanDbContext(DbContextOptions<PlanDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MealPlan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.DietStyle).HasMaxLength(100);
            entity.Property(e => e.Goal).HasMaxLength(100);
            entity.Property(e => e.AvoidTags).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        });

        modelBuilder.Entity<DailyMeal>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne<MealPlan>()
                  .WithMany(p => p.Days)
                  .HasForeignKey("MealPlanId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DailyMeal>()
            .HasMany(d => d.Breakfast)
            .WithMany()
            .UsingEntity(j => j.ToTable("DailyMealBreakfast"));

        modelBuilder.Entity<DailyMeal>()
            .HasMany(d => d.Lunch)
            .WithMany()
            .UsingEntity(j => j.ToTable("DailyMealLunch"));

        modelBuilder.Entity<DailyMeal>()
            .HasMany(d => d.Dinner)
            .WithMany()
            .UsingEntity(j => j.ToTable("DailyMealDinner"));

        modelBuilder.Entity<DailyMeal>()
            .HasMany(d => d.Snacks)
            .WithMany()
            .UsingEntity(j => j.ToTable("DailyMealSnacks"));
    }
}
