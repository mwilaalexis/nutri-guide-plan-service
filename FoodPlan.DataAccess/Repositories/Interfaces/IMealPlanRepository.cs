using FoodPlan.DataAccess.Entities;

namespace FoodPlan.DataAccess.Application.Interfaces;

public interface IMealPlanRepository
{
    Task AddAsync(MealPlan plan, CancellationToken ct = default);
    Task<MealPlan?> GetByIdAsync(Guid planId, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid planId, CancellationToken ct = default);
    Task<IEnumerable<MealPlan>> GetPlansForUserAsync(string userId, CancellationToken ct);
    Task<bool> DeleteAsync(Guid planId, CancellationToken ct = default);
    Task UpdateAsync(MealPlan plan, CancellationToken ct = default);
}