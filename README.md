
# 🗓️ Food Plan Service

A scalable **.NET 8 Web API** designed to manage **meal planning and food scheduling**, built using a clean **3-layer architecture** and designed to integrate into a **microservices ecosystem**.

---

## 🚀 Project Vision

This service is part of a larger system (**NutriGuide**) that helps users:

* Plan their meals 📅
* Track nutrition 🍽️
* Organize food consumption intelligently 🧠

👉 The **Food Plan Service** is responsible for transforming raw food data into **structured, user-specific meal plans**.

---

## 🏗️ Architecture

This project follows a **3-layer architecture** to ensure maintainability, scalability, and separation of concerns:

```text
Presentation Layer   → API Controllers (HTTP endpoints)
Business Layer       → Services, business rules, validation
Data Access Layer    → EF Core, repositories, persistence
```

---

## 🔗 System Context (Microservices)

This service is designed to work alongside other services:

```text
Client Application
        ↓
   API Gateway (optional)
        ↓
 ┌───────────────┬────────────────────┬────────────────────┐
 │ Auth Service  │ Food Service       │ Food Plan Service  │
 │ (Users)       │ (Foods/Ingredients)│ (Meal Planning)    │
 └───────────────┴────────────────────┴────────────────────┘
```

### Responsibilities

| Service           | Role                            |
| ----------------- | ------------------------------- |
| Auth Service      | Manages users & authentication  |
| Food Service      | Provides food & ingredient data |
| Food Plan Service | Builds and manages meal plans   |

---

## ⚙️ Tech Stack

* **.NET 8**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **AutoMapper**
* **SQL Server**
* **RESTful APIs**

---

## 🔑 Features

### 🗓️ Meal Plans (Full CRUD)

* Create meal plans
* Retrieve all plans
* Get plan by ID
* Update plans
* ❌ Delete plans *(removes all associated items)*

---

### 🍽️ Plan Items (Full CRUD)

* Add food to a plan
* Assign:

  * Day (Monday → Sunday)
  * Meal type (Breakfast, Lunch, Dinner, Snack)
* Update assigned meals
* Remove meals from a plan

---

## 🧠 Business Logic Highlights

This service goes beyond basic CRUD by enforcing key rules:

* Prevent duplicate meals in the same time slot
* Structured meal assignment (day + meal type)
* Separation between plan data and food data (microservices-friendly)

---

## 📡 API Design

### Meal Plans

| Method | Endpoint            | Description                 |
| ------ | ------------------- | --------------------------- |
| GET    | /api/mealplans      | Get all meal plans          |
| GET    | /api/mealplans/{id} | Get a specific plan         |
| POST   | /api/mealplans      | Create a new plan           |
| PUT    | /api/mealplans/{id} | Update an existing plan     |
| DELETE | /api/mealplans/{id} | Delete a plan and its items |

---

### Plan Items

| Method | Endpoint                           | Description           |
| ------ | ---------------------------------- | --------------------- |
| GET    | /api/mealplans/{id}/items          | Get all meals in plan |
| POST   | /api/mealplans/{id}/items          | Add food to plan      |
| PUT    | /api/mealplans/{id}/items/{itemId} | Update meal           |
| DELETE | /api/mealplans/{id}/items/{itemId} | Remove meal           |

---

## 🧪 Example Use Case

### Weekly Meal Plan

```json
{
  "day": "Monday",
  "mealType": "Lunch",
  "foodId": "chicken-rice-bowl"
}
```

➡️ Each plan item references a **Food ID** from the Food Service.

---

## ⚠️ Important Design Decisions

* ❌ Deleting a plan does NOT delete foods
* 🔗 Food data is external (handled by Food Service)
* 🔐 User identity is handled by Auth Service

👉 This ensures **loose coupling between services**

---

## 📦 Installation

```bash
git clone https://github.com/mwilaalexis/FoodPlanService.git
cd FoodPlanService
dotnet restore
```

Configure:

```json
"ConnectionStrings": {
  "DefaultConnection": "your_connection_string"
}
```

---

## ▶️ Run the Project

```bash
dotnet run
```

Swagger:

```text
https://localhost:{port}/swagger
```

---

## 🧪 Testing

* Swagger UI
* Postman
* REST Client

---

## 📈 Future Improvements

* 🤖 Smart meal plan generation (rules/AI)
* 🔥 Calorie & macro tracking
* 🛒 Shopping list generation
* 🎯 Personalized plans (diet, allergies)
* 📊 Weekly analytics

---

## 🧑‍💻 Author

**Alexis Mwila**
GitHub: [https://github.com/mwilaalexis](https://github.com/mwilaalexis)

---

## ⭐ Why This Project Stands Out

* Real-world architecture (microservices-ready)
* Clean separation of concerns
* Full CRUD + structured domain logic
* Scalable design for future features

---

## 📄 License

MIT License
