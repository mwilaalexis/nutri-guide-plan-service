namespace FoodPlan.Core.Dto;

public class PlanSummaryDto
{
    public Guid PlanId { get; set; }

    public DateOnly StartDate { get; set; }
    public int NumberOfDays { get; set; }
    public decimal TotalCalories { get; set; }
    public decimal AverageCaloriesPerDay { get; set; }

    public string DietStyle { get; set; } = string.Empty;
    public string Goal { get; set; } = string.Empty;
    public List<string> AvoidTags { get; set; } = new();
    public DateTime GeneratedAt { get; set; }
    public string Status { get; set; } = "completed";

    public List<DailyMealSummaryDto> Days { get; set; } = new();
}
