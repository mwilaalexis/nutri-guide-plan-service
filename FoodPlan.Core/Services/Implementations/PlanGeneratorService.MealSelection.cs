using FoodPlan.Core.Extensions;
using FoodPlan.DataAccess.Entities;

namespace FoodPlan.Core.Services.Implementations;

public partial class PlanGeneratorService
{

    private Meal SelectMealUsingNutritionGuide(List<FoodItemSummary> foods, string mealType, MealPlan plan, decimal targetCalories,string goal)
    {
        var selected = foods[_random.Next(foods.Count)];

        var meal = new Meal
        {
            Id = Guid.NewGuid(),
            Name = selected.Name,
            Calories = selected.Calories,
            imagePath = selected.ImagePath,
            FoodId= selected.Id
        };

        return meal;
    }


    /// <summary>Aligns meal-type strings from the API, DB, and food tags (e.g. snack vs snacks).</summary>
    private static string NormalizeMealTypeKey(string mealType)
    {
        var t = mealType.Trim().ToLowerInvariant();
        return t switch
        {
            "snacks" => "snack",
            _ => t
        };
    }

    private static double GetSlotCalorieFraction(string canonicalMealTypeLower) =>
        canonicalMealTypeLower switch
        {
            "breakfast" => 0.25,
            "lunch" => 0.35,
            "dinner" => 0.30,
            "snack" => 0.10,
            _ => 0.25
        };

    private double ScoreFoodForSlot(
        FoodItemSummary food,
        double slotCalorieBudget,
        HashSet<Guid> usedFoodIds,
        bool loseWeightGoal,
        bool gainWeightGoal)
    {
        var cal = Math.Max(food.Calories, 1e-3);
        var denom = Math.Max(slotCalorieBudget * 0.45, 80);
        var calorieFit = 100.0 / (1.0 + Math.Abs(cal - slotCalorieBudget) / denom);

        var diversity = usedFoodIds.Contains(food.Id) ? -45.0 : 0.0;

        double goalTerm;
        if (loseWeightGoal)
        {
            var proteinDensity = food.Protein * 4.0 / cal;
            goalTerm = proteinDensity * 55.0;
        }
        else if (gainWeightGoal)
        {
            var densityVsSlot = Math.Min(cal, slotCalorieBudget * 1.35) / Math.Max(slotCalorieBudget, 1);
            goalTerm = densityVsSlot * 40.0;
        }
        else
        {
            goalTerm = BalancedMacroScore(food) * 35.0;
        }

        var tieBreak = _random.NextDouble() * 3.0;
        return calorieFit + diversity + goalTerm + tieBreak;
    }

    private static double BalancedMacroScore(FoodItemSummary food)
    {
        var pCal = food.Protein * 4.0;
        var cCal = food.Carbs * 4.0;
        var fCal = food.Fat * 9.0;
        var total = pCal + cCal + fCal;
        if (total < 1)
            return 0.35;

        var p = pCal / total;
        var c = cCal / total;
        var f = fCal / total;
        const double tp = 0.28, tc = 0.47, tf = 0.25;
        var deviation = Math.Abs(p - tp) + Math.Abs(c - tc) + Math.Abs(f - tf);
        return Math.Clamp(1.0 - deviation * 0.9, 0, 1);
    }

    private static Meal CreateMealFromFood(FoodItemSummary food, string canonicalMealTypeLower) =>
        new Meal
        {
            Id = Guid.NewGuid(),
            Type = canonicalMealTypeLower,
            Name = food.Name,
            Calories = food.Calories,
            imagePath = food.ImagePath,
            FoodId = food.Id
        };

    private Meal GetSpecificMeal(List<FoodItemSummary> foods, Guid foodId, string originalMealType)
    {
        var selected = foods.FirstOrDefault(f => f.Id == foodId)
            ?? throw new InvalidOperationException("Selected food is not compatible with this plan.");

        return CreateMealFromFood(selected, NormalizeMealTypeKey(originalMealType));
    }
}
