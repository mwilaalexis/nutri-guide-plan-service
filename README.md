# NutriGuide - Meal Plan Service

Backend API to create and manage personalized meal plans.

## What this project does

Users (or the app) can build **meal plans** made of **plan items** linked to foods from the Food Service. This is the planning layer of NutriGuide.

## Main features

- Create, read, update, and delete meal plans
- Manage items inside a plan
- Connect to food data for realistic plans
- Swagger for testing endpoints

## Technologies

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- AutoMapper

## Where this fits in NutriGuide

```
Client -> API Gateway -> Plan Service (this repo)
                              |
                              v
                        Food Service (catalog)
```

## Prerequisites

- .NET 8 SDK
- SQL Server
- Food Service running (recommended for full tests)

## Run locally

1. Clone the repository:
   ```bash
   git clone https://github.com/mwilaalexis/nutri-guide-plan-service.git
   cd nutri-guide-plan-service
   ```
2. Configure `PlanDb` connection string in `WebApplication2/appsettings.json`.
3. Run the API:
   ```bash
   dotnet run --project WebApplication2
   ```
4. Open Swagger: `https://localhost:7028/swagger` or `http://localhost:5062/swagger`.

## API overview

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/plans` | List plans |
| GET | `/api/plans/{id}` | Get plan by id |
| POST | `/api/plans` | Create plan |
| PUT | `/api/plans/{id}` | Update plan |
| DELETE | `/api/plans/{id}` | Delete plan |

Plan items are usually managed under related endpoints (see Swagger).

## Suggested folder structure

```
nutri-guide-plan-service/
├── WebApplication2/      # API layer
├── FoodPlan.Core/        # Business logic, DTOs
├── FoodPlan.DataAccess/  # Database access
└── README.md
```

## Skills demonstrated

- Business logic separate from controllers
- EF Core and relational data modeling
- HTTP client calls to another service (if configured)
- End-to-end feature in a student capstone-style project

## Ideas to improve for recruiters

- [ ] Add one worked example: JSON body for `POST /api/plans`
- [ ] Clarify dependency on Food Service in a `SETUP.md`
- [ ] Add simple error messages for missing food ids
- [ ] GitHub Action: build on push

## Related repositories

- [nutri-guide-gateway](https://github.com/mwilaalexis/nutri-guide-gateway)
- [nutri-guide-user-service](https://github.com/mwilaalexis/nutri-guide-user-service)
- [nutri-guide-food-service](https://github.com/mwilaalexis/nutri-guide-food-service)
- [nutri-guide-web](https://github.com/mwilaalexis/nutri-guide-web)

## Author

**Alex Mwila** - [@mwilaalexis](https://github.com/mwilaalexis)
