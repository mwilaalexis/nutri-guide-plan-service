namespace FoodPlan.Core.Dto;

public class MealSummaryDto
{
    public Guid MealId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Calories { get; set; }
    public string? ImagePath { get; set; }
    public Guid FoodId { get; set; }

    public List<MealIngredientDto> Ingredients { get; set; } = new();
    public object Type { get; internal set; }
}

