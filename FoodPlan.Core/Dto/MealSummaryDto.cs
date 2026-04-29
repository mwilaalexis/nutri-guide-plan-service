namespace FoodPlan.Core.Dto;

public class MealSummaryDto
{
    public Guid MealId { get; set; }
    public string Type { get; set; } = string.Empty; // breakfast, lunch, dinner, snack
    public string Name { get; set; } = string.Empty;
    public decimal Calories { get; set; }
    public string ImagePath { get; set; }
    public Guid FoodId { get; set; }
}
