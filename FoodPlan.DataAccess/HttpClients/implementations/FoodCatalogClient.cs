using FoodPlan.DataAccess.Application.Interfaces;
using System.Net.Http.Json;
using FoodPlan.DataAccess.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace WebApplication2.Services.Implementations;

public class FoodCatalogClient : IFoodCatalogClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FoodCatalogClient> _logger;

    public FoodCatalogClient(HttpClient httpClient, IConfiguration configuration, ILogger<FoodCatalogClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        var baseUrl = configuration["FoodService:BaseUrl"] ?? "http://localhost:5001";
        _httpClient.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
    }

    public async Task<List<FoodItemSummary>> GetCompatibleFoodsAsync(string dietStyle,IEnumerable<string> avoidTags,CancellationToken ct = default)
    {
        try
        {
            dietStyle ??= string.Empty;
            var query = $"?dietStyle={Uri.EscapeDataString(dietStyle)}" +
                        $"&avoidTags={string.Join(",", avoidTags)}";

            var response = await _httpClient.GetFromJsonAsync<List<FoodItemSummary>>(
                $"api/foods/compatible{query}", ct);

            return response ?? new List<FoodItemSummary>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch compatible foods");
            return new List<FoodItemSummary>();
        }
    }
}