
namespace FoodPlan.DataAccess.Entities;

public class DailyMeal
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateOnly Date { get; set; }
    public decimal TotalCalories { get; set; }
 

    public List<Meal> Breakfast { get; set; } = new();
    public List<Meal> Lunch { get; set; } = new();
    public List<Meal> Dinner { get; set; } = new();
    public List<Meal> Snacks { get; set; } = new();

  
}
