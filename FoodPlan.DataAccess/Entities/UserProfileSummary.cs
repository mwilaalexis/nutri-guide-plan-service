namespace FoodPlan.DataAccess.Entities
{
    public class UserProfileSummary
    {
        public int? Age { get; set; }
        public decimal? WeightKg { get; set; }
        public decimal? HeightCm { get; set; }
        public string? BiologicalSex { get; set; }
        public string? ActivityLevel { get; set; }
        public string Goal { get; set; }
        public string DietType { get; set; }
        public List<string> Allergies { get; set; }
    }
}
