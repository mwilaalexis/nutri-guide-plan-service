using FoodPlan.DataAccess.Entities;

namespace FoodPlan.DataAccess.Application.Interfaces;

public interface IFoodCatalogClient
{
    Task<List<FoodItemSummary>> GetCompatibleFoodsAsync(string dietStyle, IEnumerable<string> avoidTags, CancellationToken ct = default);
}

