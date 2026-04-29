using FoodPlan.DataAccess.Entities;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FoodPlan.Core.Extensions
{
    public static class MealPlanExtensins
    {
        public static IEnumerable<Meal> GetAllMeals( this DailyMeal dailyMeal)
        {
            return new List<Meal>()
                .Concat(dailyMeal.Breakfast)
                .Concat(dailyMeal.Lunch)
                .Concat(dailyMeal.Dinner)
                .Concat(dailyMeal.Snacks);
        }
        public static MealPlan Clone(this MealPlan mealPlan)
        {
               var clone = new MealPlan
            {
                Id = Guid.NewGuid(),
                UserId = mealPlan.UserId,
                StartDate = mealPlan.StartDate,
                NumberOfDays = mealPlan.NumberOfDays,
                TargetDailyCalories = mealPlan.TargetDailyCalories,
                DietStyle = mealPlan.DietStyle,
                Goal = mealPlan.Goal,
                AvoidTags = new List<string>(mealPlan.AvoidTags),
                GeneratedAt = DateTime.UtcNow,
                Status = "duplicated"
            };

            foreach (var day in mealPlan.Days)
            {
                var newDay = new DailyMeal
                {
                    Id = Guid.NewGuid(),
                    Date = day.Date,
                    TotalCalories = day.TotalCalories
                };

                // Clone Breakfast
                foreach (var meal in day.Breakfast)
                {
                    newDay.Breakfast.Add(new Meal
                    {
                        Id = Guid.NewGuid(),
                        Type = meal.Type,
                        Name = meal.Name,
                        Calories = meal.Calories
                    });
                }

                // Clone Lunch
                foreach (var meal in day.Lunch)
                {
                    newDay.Lunch.Add(new Meal
                    {
                        Id = Guid.NewGuid(),
                        Type = meal.Type,
                        Name = meal.Name,
                        Calories = meal.Calories
                    });
                }

                // Clone Dinner
                foreach (var meal in day.Dinner)
                {
                    newDay.Dinner.Add(new Meal
                    {
                        Id = Guid.NewGuid(),
                        Type = meal.Type,
                        Name = meal.Name,
                        Calories = meal.Calories
                    });
                }

                // Clone Snacks
                foreach (var meal in day.Snacks)
                {
                    newDay.Snacks.Add(new Meal
                    {
                        Id = Guid.NewGuid(),
                        Type = meal.Type,
                        Name = meal.Name,
                        Calories = meal.Calories
                    });
                }

                clone.Days.Add(newDay);
            }

            return clone;
        }
        public static Meal FindMealById(this MealPlan plan, Guid mealId)
        {
            foreach (var day in plan.Days)
            {
                var meal =
                    day.Breakfast.FirstOrDefault(m => m.Id == mealId) ??
                    day.Lunch.FirstOrDefault(m => m.Id == mealId) ??
                    day.Dinner.FirstOrDefault(m => m.Id == mealId) ??
                    day.Snacks.FirstOrDefault(m => m.Id == mealId);

                if (meal != null)
                {
                    return meal;
                }
            }

            return null;
        }

    }
}

