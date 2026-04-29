using FoodPlan.Core.Services.Implementations;
using FoodPlan.COre.Application.Interfaces;
using FoodPlan.DataAccess.Application.Interfaces;
using FoodPlan.DataAccess.Data;
using FoodPlan.DataAccess.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebApplication2.Mappings;
using WebApplication2.Services.Implementations;

namespace WebApplication2.Extensions
{
    public static class ApplicationDependencies
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IPlanGeneratorService, PlanGeneratorService>();
            builder.Services.AddScoped<IMealPlanRepository, MealPlanRepository>();
            builder.Services.AddScoped<IFoodCatalogClient, FoodCatalogClient>();
            builder.Services.AddScoped<IProfileClient, ProfileClient>();

            builder.Services.AddDbContext<PlanDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PlanDb")));
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.AddHttpClientWithPollyResilience();

            // Register authentication & authorization once (safe if called multiple times)
            builder.ConfigureAuthentication();
            builder.ConfigureAuthorizationPolicy();
        }


        public static void ConfigureAuthorizationPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("BasicUserAccess", policy =>
                    policy.RequireRole("User", "Moderator", "Admin"));

                options.AddPolicy("ContentManager", policy =>
                    policy.RequireRole("Moderator", "Admin"));

                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));
            });
        }

        public static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

                        NameClaimType = "sub",
                        RoleClaimType = ClaimTypes.Role,

                        ClockSkew = TimeSpan.FromMinutes(5)
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices
                                .GetRequiredService<ILoggerFactory>()
                                .CreateLogger("JwtAuth");

                            logger.LogWarning(context.Exception, "JWT authentication failed");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var logger = context.HttpContext.RequestServices
                                .GetRequiredService<ILoggerFactory>()
                                .CreateLogger("JwtAuth");

                            var claims = context.Principal?.Claims
                                .Select(c => $"{c.Type}={c.Value}")
                                .ToArray() ?? Array.Empty<string>();

                            logger.LogInformation("Token validated. Claims: {claims}", claims);
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        public static void AddHttpClientWithPollyResilience(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient<IFoodCatalogClient, FoodCatalogClient>()
                .AddStandardResilienceHandler();

            builder.Services.AddHttpClient<IProfileClient, ProfileClient>()
              .AddStandardResilienceHandler();
        }
    }
}