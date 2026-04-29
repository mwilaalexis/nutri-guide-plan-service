using System.Text.Json.Serialization;

namespace FoodPlan.Core.Dto
{
    public class PlanRequest
    {
        [JsonIgnore]
        public Guid UserId {  get; set; }
        public DateOnly StartDate { get; set; }
        public int NumberOfDays { get; set; } = 7;
        public string Notes { get; set; }
    }
}
