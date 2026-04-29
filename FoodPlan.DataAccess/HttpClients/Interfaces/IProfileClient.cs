using FoodPlan.DataAccess.Entities;

namespace FoodPlan.DataAccess.Application.Interfaces;

public interface IProfileClient
{
    Task<UserProfileSummary?> GetProfileAsync(Guid userId, CancellationToken ct = default);
}

