namespace FoodPlan.DataAccess.Entities;

public class MealPlan
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public DateOnly StartDate { get; set; }
    public int NumberOfDays { get; set; }
    public decimal TargetDailyCalories { get; set; }
    public string DietStyle { get; set; } = string.Empty;
    public string Goal { get; set; } = string.Empty;
    public List<string> AvoidTags { get; set; } = new();
    public List<DailyMeal> Days { get; set; } = new();
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "completed";
}
