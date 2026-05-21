using FoodPlan.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using WebApplication2.Extensions;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();


app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("RequestAuth");
    logger.LogInformation("➡ Received {method} {path}. Authorization: {authHeader}",
        context.Request.Method, context.Request.Path,
        context.Request.Headers["Authorization"].ToString().Length > 0 ? "present" : "none");

    logger.LogInformation("User.IsAuthenticated={IsAuthenticated}; Name={Name}; Claims={Claims}",
        context.User?.Identity?.IsAuthenticated ?? false,
        context.User?.Identity?.Name ?? "<none>",
        string.Join(", ", context.User?.Claims.Select(c => $"{c.Type}={c.Value}") ?? Array.Empty<string>()));

    await next();
});

app.MapControllers();

app.Run();