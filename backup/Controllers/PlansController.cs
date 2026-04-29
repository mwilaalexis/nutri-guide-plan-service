using FoodPlan.Core.Dto;
using FoodPlan.COre.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/plans")]

    public class PlansController : ControllerBase
    {
        private readonly IPlanGeneratorService _service;

        public PlansController(IPlanGeneratorService service)
        {
            _service = service;
        }

        private string? GetUserId()
        {
            return User.FindFirstValue("sub")
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("userId");
        }

        [Authorize]
        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] PlanRequest request)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            request.UserId = Guid.Parse(userId);
            var plan = await _service.GeneratePlanAsync(request, HttpContext.RequestAborted);
            return Ok(plan);
        }

        [HttpGet("{planId:Guid}")]
        public async Task<IActionResult> GetPlan(Guid planId)
        {
            var plan = await _service.GetPlanByIdAsync(planId);
            if (plan == null)
                return NotFound();

            return Ok(plan);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllPlans()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var plans = await _service.GetPlansForUserAsync(userId);
            return Ok(plans);
        }

        [HttpDelete("{planId:guid}")]
        public async Task<IActionResult> DeletePlan(Guid planId)
        {
            var success = await _service.DeletePlanAsync(planId);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{planId:guid}/swap/{mealId:guid}/{preferredFoodId:guid}")]
        public async Task<IActionResult> SwapMeal(Guid planId, Guid mealId, Guid? preferredFoodId = null)
        {
            var updated = await _service.SwapMealAsync(planId, mealId, preferredFoodId);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpPut("{planId:guid}/regenerate/day/{dayIndex:int}")]
        public async Task<IActionResult> RegenerateDay(Guid planId, int dayIndex)
        {
            var updated = await _service.RegenerateDayAsync(planId, dayIndex);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpPut("{planId:guid}/regenerate/meal/{mealId:guid}")]
        public async Task<IActionResult> RegenerateMeal(Guid planId, Guid mealId)
        {
            var updated = await _service.RegenerateMealAsync(planId, mealId);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpGet("{planId:guid}/summary")]
        public async Task<IActionResult> GetSummary(Guid planId)
        {
            var summary = await _service.GetPlanSummaryAsync(planId);
            if (summary == null)
                return NotFound();

            return Ok(summary);
        }

        [HttpPost("{planId:guid}/duplicate")]
        public async Task<IActionResult> Duplicate(Guid planId)
        {
            var duplicated = await _service.DuplicatePlanAsync(planId);
            if (duplicated == null)
                return NotFound();

            return Ok(duplicated);
        }
    }
}