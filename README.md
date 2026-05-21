# Food Plan Service

Meal planning microservice for the [NutriGuide](https://github.com/mwilaalexis/NutriGuideUI) platform. Builds and manages structured meal plans from food and ingredient data.

## Responsibilities

- Create, read, update, and delete meal plans
- Manage plan items (meals linked to foods)
- Integrate with the food catalog service
- Support plan export workflows

## Tech stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- AutoMapper

## Architecture

Three-layer structure:

```
Presentation (API)  ->  Business (services)  ->  Data (EF Core, repositories)
```

## System context

```
Client / NutriGuideUI
        |
   NutriGuidGateway
        |
   +----+-------------+------------------+
   | Auth / Profile  | Food / Ingredient | Food Plan (this repo)
   +-----------------+-------------------+
```

## Features

- Full CRUD on meal plans
- Plan items linked to catalog entries
- Swagger/OpenAPI for API exploration

## Run locally

```bash
dotnet run
```

Set `PlanDb` in `appsettings.json`. Gateway base URL for downstream calls is typically `http://localhost:5284`.

## Related repositories

| Service | Repository |
|---------|------------|
| Gateway | [NutriGuidGateway](https://github.com/mwilaalexis/NutriGuidGateway) |
| Food catalog | [Food-IngredientService](https://github.com/mwilaalexis/Food-IngredientService) |
| Auth and profiles | [AuthAndUserProfileService](https://github.com/mwilaalexis/AuthAndUserProfileService) |
| Frontend | [NutriGuideUI](https://github.com/mwilaalexis/NutriGuideUI) |
