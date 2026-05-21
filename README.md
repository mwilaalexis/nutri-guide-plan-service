# NutriGuide Plan Service

Meal planning microservice for the [NutriGuide](https://github.com/mwilaalexis/nutri-guide-web) platform. Builds and manages structured meal plans from food and ingredient data.

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
Client / nutri-guide-web
        |
   nutri-guide-gateway
        |
   +----+-------------+------------------+
   | User service     | Food service     | Plan service (this repo)
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
| Gateway | [nutri-guide-gateway](https://github.com/mwilaalexis/nutri-guide-gateway) |
| Food catalog | [nutri-guide-food-service](https://github.com/mwilaalexis/nutri-guide-food-service) |
| User service | [nutri-guide-user-service](https://github.com/mwilaalexis/nutri-guide-user-service) |
| Frontend | [nutri-guide-web](https://github.com/mwilaalexis/nutri-guide-web) |
