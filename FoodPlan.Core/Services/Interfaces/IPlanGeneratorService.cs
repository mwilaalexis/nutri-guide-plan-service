using FoodPlan.Core.Dto;

namespace FoodPlan.COre.Application.Interfaces;

public interface IPlanGeneratorService
{

    Task<bool> DeletePlanAsync(Guid planId, CancellationToken ct = default);
    Task<PlanSummaryDto?> DuplicatePlanAsync(Guid planId, CancellationToken ct = default);
    public Task<PlanSummaryDto?> GeneratePlanAsync(PlanRequest message, CancellationToken cancellationToken);
    public Task<PlanSummaryDto?> GenerateWeeklyPlanAsync(PlanRequest request, CancellationToken ct = default);
    Task<PlanSummaryDto?> GetPlanByIdAsync(Guid planId, CancellationToken ct = default);
    Task<IEnumerable<PlanSummaryDto>> GetPlansForUserAsync(string userId, CancellationToken ct = default);
    Task<PlanSummaryDto?> GetPlanSummaryAsync(Guid planId, CancellationToken ct = default);
    Task<PlanSummaryDto?> RegenerateDayAsync(Guid planId, int dayIndex, CancellationToken ct = default);
    Task<PlanSummaryDto?> RegenerateMealAsync(Guid planId, Guid mealId, CancellationToken ct = default);
    Task<PlanSummaryDto?> SwapMealAsync(Guid planId, Guid mealId, Guid? preferredFoodId = null, CancellationToken ct = default);
}