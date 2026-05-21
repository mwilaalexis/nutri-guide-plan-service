namespace FoodPlan.DataAccess.Entities
{
    public class Meal
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string imagePath { get; set; } = string.Empty;
        public double Calories { get; set; }
        public Guid FoodId { get; set; } = Guid.NewGuid();
        public List<MealIngredient> Ingredients { get; set; } = new();
    }


}