using FoodPlan.DataAccess.Application.Interfaces;
using System.Net.Http.Json;
using FoodPlan.DataAccess.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace FoodPlan.DataAccess.Services.Implementations;

public class ProfileClient : IProfileClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProfileClient> _logger;

    public ProfileClient(HttpClient httpClient, IConfiguration configuration, ILogger<ProfileClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        var baseUrl = configuration["ProfileService:BaseUrl"] ?? "http://localhost:5002";
        _httpClient.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
    }

    public async Task<UserProfileSummary?> GetProfileAsync(Guid userId, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<UserProfileSummary>(
                $"api/profile/{userId}", ct);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch profile for user {UserId}", userId);
            return null;
        }
    }
}