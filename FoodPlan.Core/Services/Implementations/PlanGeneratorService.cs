using AutoMapper;
using FoodPlan.Core.Dto;
using FoodPlan.Core.Extensions;
using FoodPlan.COre.Application.Interfaces;
using FoodPlan.DataAccess.Application.Interfaces;
using FoodPlan.DataAccess.Entities;
using Microsoft.Extensions.Logging;

namespace FoodPlan.Core.Services.Implementations;

public partial class PlanGeneratorService : IPlanGeneratorService
{
    private readonly IMealPlanRepository _repository;
    private readonly IFoodCatalogClient _foodClient;
    private readonly IProfileClient _profileClient;
    private readonly ILogger<PlanGeneratorService> _logger;
    private readonly IMapper _mapper;

    private static readonly Random _random = new Random();

    public PlanGeneratorService(IMealPlanRepository repository, IFoodCatalogClient foodClient, IProfileClient profileClient, ILogger<PlanGeneratorService> logger, IMapper mapper)
    {
        _repository = repository;
        _foodClient = foodClient;
        _profileClient = profileClient;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<PlanSummaryDto?> GeneratePlanAsync(PlanRequest request, CancellationToken ct = default)
    {
        _logger.LogInformation("Starting plan generation for user {UserId}", request.UserId);

        var profile = await _profileClient.GetProfileAsync(request.UserId, ct)
            ?? throw new InvalidOperationException($"Profile not found for user {request.UserId}");

        decimal targetCalories = CalculateDailyCalories(profile);
        var avoidTags = profile.Allergies ?? new List<string>();

        var foods = await _foodClient.GetCompatibleFoodsAsync(profile.DietType, avoidTags, ct);
        if (!foods.Any())
            throw new InvalidOperationException("No compatible foods found for the user's diet and restrictions.");

        var plan = new MealPlan
        {
            UserId = request.UserId,
            StartDate = request.StartDate,
            NumberOfDays = request.NumberOfDays,
            TargetDailyCalories = targetCalories,
            DietStyle = profile.DietType,
            Goal = profile.Goal,
            AvoidTags = avoidTags,
            Status = "Active"
        };

        for (int day = 0; day < request.NumberOfDays; day++)
        {
            var currentDate = plan.StartDate.AddDays(day);
            var daily = new DailyMeal
            {
                Date = currentDate,
                TotalCalories = targetCalories
            };

            daily.Breakfast.Add(SelectMealUsingNutritionGuide(foods, "breakfast", plan, targetCalories, profile.Goal));
            daily.Lunch.Add(SelectMealUsingNutritionGuide(foods, "lunch", plan, targetCalories, profile.Goal));
            daily.Dinner.Add(SelectMealUsingNutritionGuide(foods, "dinner", plan, targetCalories, profile.Goal));
            daily.Snacks.Add(SelectMealUsingNutritionGuide(foods, "snack", plan, targetCalories, profile.Goal));

            plan.Days.Add(daily);
        }

        await _repository.AddAsync(plan, ct);

        return _mapper.Map<PlanSummaryDto>(plan);
    }

    public async Task<PlanSummaryDto?> GenerateWeeklyPlanAsync(PlanRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        request.NumberOfDays = 7;
        request.StartDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);

        return await GeneratePlanAsync(request, ct);
    }

    public async Task<PlanSummaryDto?> SwapMealAsync(Guid planId, Guid mealId, Guid? preferredFoodId = null, CancellationToken ct = default)
    {
        var plan = await _repository.GetByIdAsync(planId, ct);
        if (plan == null) return null;

        var meal = plan.FindMealById(mealId);
        if (meal == null) return null;

        var compatibleFoods = await _foodClient.GetCompatibleFoodsAsync(plan.DietStyle, plan.AvoidTags, ct);
        if (compatibleFoods.Count == 0)
            throw new InvalidOperationException("No compatible foods found for this plan's diet and restrictions.");

        var newMeal = preferredFoodId.HasValue
            ? GetSpecificMeal(compatibleFoods, preferredFoodId.Value, meal.Type)
            : SelectMealUsingNutritionGuide(compatibleFoods, meal.Type, plan, plan.TargetDailyCalories, plan.Goal);


        meal.Name = newMeal.Name;
        meal.Calories = newMeal.Calories;
        meal.imagePath = newMeal.imagePath;
        meal.FoodId = newMeal.FoodId;

        await _repository.UpdateAsync(plan, ct);

        return _mapper.Map<PlanSummaryDto>(plan);
    }

    public async Task<PlanSummaryDto?> RegenerateDayAsync(Guid planId, int dayIndex, CancellationToken ct = default)
    {
        var plan = await _repository.GetByIdAsync(planId, ct);
        if (plan == null || dayIndex < 0 || dayIndex >= plan.Days.Count)
            return null;

        var foods = await _foodClient.GetCompatibleFoodsAsync(plan.DietStyle, plan.AvoidTags, ct);
        if (foods.Count == 0)
            throw new InvalidOperationException("No compatible foods found for this plan's diet and restrictions.");

        var day = plan.Days[dayIndex];

        day.Breakfast.Clear();
        day.Lunch.Clear();
        day.Dinner.Clear();
        day.Snacks.Clear();

        day.Breakfast.Add(SelectMealUsingNutritionGuide(foods, "breakfast", plan, plan.TargetDailyCalories, plan.Goal));
        day.Lunch.Add(SelectMealUsingNutritionGuide(foods, "lunch", plan, plan.TargetDailyCalories, plan.Goal));
        day.Dinner.Add(SelectMealUsingNutritionGuide(foods, "dinner", plan, plan.TargetDailyCalories, plan.Goal));
        day.Snacks.Add(SelectMealUsingNutritionGuide(foods, "snack", plan, plan.TargetDailyCalories, plan.Goal));

        await _repository.UpdateAsync(plan, ct);

        return _mapper.Map<PlanSummaryDto>(plan);
    }

    public async Task<PlanSummaryDto?> RegenerateMealAsync(Guid planId, Guid mealId, CancellationToken ct = default)
    {
        var plan = await _repository.GetByIdAsync(planId, ct);
        if (plan == null) return null;

        var meal = plan.FindMealById(mealId);
        if (meal == null) return null;

        var foods = await _foodClient.GetCompatibleFoodsAsync(plan.DietStyle, plan.AvoidTags, ct);
        if (foods.Count == 0)
            throw new InvalidOperationException("No compatible foods found for this plan's diet and restrictions.");

        var newMeal = SelectMealUsingNutritionGuide(foods, meal.Type, plan, plan.TargetDailyCalories, plan.Goal);

        meal.Name = newMeal.Name;
        meal.Calories = newMeal.Calories;
        meal.imagePath = newMeal.imagePath;
        meal.FoodId = newMeal.FoodId;

        await _repository.UpdateAsync(plan, ct);

        return _mapper.Map<PlanSummaryDto>(plan);
    }

    public async Task<PlanSummaryDto?> GetPlanSummaryAsync(Guid planId, CancellationToken ct = default)
    {
        var plan = await _repository.GetByIdAsync(planId, ct);
        if (plan == null)
            return null;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var lastPlanDate = plan.StartDate.AddDays(Math.Max(0, plan.NumberOfDays - 1));
        plan.Status = today > lastPlanDate ? "Completed" : "Active";

        return _mapper.Map<PlanSummaryDto>(plan);
    }

    public async Task<PlanSummaryDto?> DuplicatePlanAsync(Guid planId, CancellationToken ct = default)
    {
        var plan = await _repository.GetByIdAsync(planId, ct);
        if (plan == null) return null;

        var clone = plan.Clone();
        clone.StartDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);

        await _repository.AddAsync(clone, ct);

        return _mapper.Map<PlanSummaryDto>(clone);
    }

    private decimal CalculateDailyCalories(UserProfileSummary profile)
    {
        decimal weight = profile.WeightKg ?? 70m;
        decimal height = profile.HeightCm ?? 170m;
        int age = profile.Age ?? 30;

        
        decimal bmr = profile.BiologicalSex?.ToLower() == "male"
            ? 10m * weight + 6.25m * height - 5m * age + 5m
            : 10m * weight + 6.25m * height - 5m * age - 161m;

        
        decimal activityMultiplier = profile.ActivityLevel switch
        {
            "Sedentary" => 1.2m,
            "Lightly Active" => 1.375m,
            "Moderately Active" => 1.55m,
            "Very Active" => 1.725m,
            _ => 1.2m
        };

      
        decimal maintenanceCalories = bmr * activityMultiplier;

        
        decimal adjustedCalories = profile.Goal switch
        {
            "Lose Weight" => maintenanceCalories - 500m,   
            "Gain Weight" => maintenanceCalories + 400m,   
            "Aggressive Loss" => maintenanceCalories - 750m,
            _ => maintenanceCalories
        };

      
        if (profile.BiologicalSex?.ToLower() == "male")
            adjustedCalories = Math.Max(adjustedCalories, 1500m);
        else
            adjustedCalories = Math.Max(adjustedCalories, 1200m);

        return Math.Round(adjustedCalories);
    }

    public async Task<bool> DeletePlanAsync(Guid planId, CancellationToken ct = default)
    {
        return await _repository.DeleteAsync(planId, ct);
    }

    public async Task<PlanSummaryDto?> GetPlanByIdAsync(Guid planId, CancellationToken ct = default)
    {
        var plan = await _repository.GetByIdAsync(planId, ct);

        return _mapper.Map<PlanSummaryDto?>(plan);

    }

    public async Task<IEnumerable<PlanSummaryDto>> GetPlansForUserAsync(string userId, CancellationToken ct = default)
    {
        var plan = await _repository.GetPlansForUserAsync(userId, ct);

        return _mapper.Map<IEnumerable<PlanSummaryDto>>(plan);
    }
}