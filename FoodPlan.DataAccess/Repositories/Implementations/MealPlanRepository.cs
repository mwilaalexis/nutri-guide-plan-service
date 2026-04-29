using FoodPlan.DataAccess.Application.Interfaces;
using FoodPlan.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using FoodPlan.DataAccess.Entities;

namespace FoodPlan.DataAccess.Services.Implementations;

public class MealPlanRepository : IMealPlanRepository
{
    private readonly PlanDbContext _context;

    public MealPlanRepository(PlanDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(MealPlan plan, CancellationToken ct = default)
    {
        await _context.MealPlans.AddAsync(plan, ct);
        await _context.SaveChangesAsync(ct);
    }

 
    public async Task<MealPlan?> GetByIdAsync(Guid planId, CancellationToken ct = default)
    {
        return await _context.MealPlans
            .Include(p => p.Days)
                .ThenInclude(d => d.Breakfast)
            .Include(p => p.Days)
                .ThenInclude(d => d.Lunch)
            .Include(p => p.Days)
                .ThenInclude(d => d.Dinner)
            .Include(p => p.Days)
                .ThenInclude(d => d.Snacks)
            .FirstOrDefaultAsync(p => p.Id == planId, ct);
    }

  
    public async Task<IEnumerable<MealPlan>> GetPlansForUserAsync(string userId, CancellationToken ct = default)
    {
        return await _context.MealPlans
            .Where(p => p.UserId ==Guid.Parse(userId))
           .Include(p => p.Days)
                .ThenInclude(d => d.Breakfast)
            .Include(p => p.Days)
                .ThenInclude(d => d.Lunch)
            .Include(p => p.Days)
                .ThenInclude(d => d.Dinner)
            .Include(p => p.Days)
                .ThenInclude(d => d.Snacks)
            .ToListAsync(ct);
    }

 
    public async Task UpdateAsync(MealPlan plan, CancellationToken ct = default)
    {
        _context.MealPlans.Update(plan);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> DeleteAsync(Guid planId, CancellationToken ct = default)
    {
        var plan = await _context.MealPlans.FindAsync(new object[] { planId }, ct);
        if (plan == null) return false;

        _context.MealPlans.Remove(plan);
        await _context.SaveChangesAsync(ct);
        return true;
    }


    public async Task<bool> ExistsAsync(Guid planId, CancellationToken ct = default)
    {
        return await _context.MealPlans.AnyAsync(p => p.Id == planId, ct);
    }
}
