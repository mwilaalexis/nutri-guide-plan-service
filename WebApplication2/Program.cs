using WebApplication2.Extensions;

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

// IMPORTANT: authentication before authorization and before controllers
app.UseAuthentication();
app.UseAuthorization();

// Diagnostic middleware placed AFTER authentication so it shows the authenticated principal
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