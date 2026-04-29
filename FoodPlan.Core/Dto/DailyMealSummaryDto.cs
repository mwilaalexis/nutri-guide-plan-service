namespace FoodPlan.Core.Dto;

public class DailyMealSummaryDto
{
    public Guid DayId { get; set; }
    public DateOnly Date { get; set; }
    public decimal TotalCalories { get; set; }
    public List<MealSummaryDto> Breakfast { get; set; } = new();
    public List<MealSummaryDto> Lunch { get; set; } = new();
    public List<MealSummaryDto> Dinner { get; set; } = new();
    public List<MealSummaryDto> Snacks { get; set; } = new();
}
