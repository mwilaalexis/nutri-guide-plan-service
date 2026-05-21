
namespace FoodPlan.DataAccess.Entities
{
    public class FoodItemSummary
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
        public List<string> Tags { get; set; }

     
    }
}
