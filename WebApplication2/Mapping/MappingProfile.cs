using AutoMapper;
using FoodPlan.Core.Dto;
using FoodPlan.DataAccess.Entities;

namespace WebApplication2.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Meal, MealSummaryDto>()
          .ForMember(dest => dest.MealId, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.imagePath));


        CreateMap<DailyMeal, DailyMealSummaryDto>()
            .ForMember(dest => dest.DayId, opt => opt.MapFrom(src => src.Id));
            


        CreateMap<MealPlan, PlanSummaryDto>()
            .ForMember(dest => dest.PlanId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NumberOfDays, opt => opt.MapFrom(src => src.NumberOfDays))
            .ForMember(dest => dest.TotalCalories, opt => opt.MapFrom(src => src.Days.Sum(d => d.TotalCalories)))
            .ForMember(dest => dest.AverageCaloriesPerDay, opt => opt.MapFrom(src => src.Days.Average(d => d.TotalCalories)));
    }
}
