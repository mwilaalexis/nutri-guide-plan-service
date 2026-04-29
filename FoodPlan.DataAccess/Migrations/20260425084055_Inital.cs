using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodPlan.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MealPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    NumberOfDays = table.Column<int>(type: "int", nullable: false),
                    TargetDailyCalories = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DietStyle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Goal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AvoidTags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calories = table.Column<double>(type: "float", nullable: false),
                    FoodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyMeals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalCalories = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MealPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyMeals_MealPlans_MealPlanId",
                        column: x => x.MealPlanId,
                        principalTable: "MealPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyMealBreakfast",
                columns: table => new
                {
                    BreakfastId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DailyMealId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMealBreakfast", x => new { x.BreakfastId, x.DailyMealId });
                    table.ForeignKey(
                        name: "FK_DailyMealBreakfast_DailyMeals_DailyMealId",
                        column: x => x.DailyMealId,
                        principalTable: "DailyMeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyMealBreakfast_Meals_BreakfastId",
                        column: x => x.BreakfastId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyMealDinner",
                columns: table => new
                {
                    DailyMeal2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DinnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMealDinner", x => new { x.DailyMeal2Id, x.DinnerId });
                    table.ForeignKey(
                        name: "FK_DailyMealDinner_DailyMeals_DailyMeal2Id",
                        column: x => x.DailyMeal2Id,
                        principalTable: "DailyMeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyMealDinner_Meals_DinnerId",
                        column: x => x.DinnerId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyMealLunch",
                columns: table => new
                {
                    DailyMeal1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LunchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMealLunch", x => new { x.DailyMeal1Id, x.LunchId });
                    table.ForeignKey(
                        name: "FK_DailyMealLunch_DailyMeals_DailyMeal1Id",
                        column: x => x.DailyMeal1Id,
                        principalTable: "DailyMeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyMealLunch_Meals_LunchId",
                        column: x => x.LunchId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyMealSnacks",
                columns: table => new
                {
                    DailyMeal3Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SnacksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMealSnacks", x => new { x.DailyMeal3Id, x.SnacksId });
                    table.ForeignKey(
                        name: "FK_DailyMealSnacks_DailyMeals_DailyMeal3Id",
                        column: x => x.DailyMeal3Id,
                        principalTable: "DailyMeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyMealSnacks_Meals_SnacksId",
                        column: x => x.SnacksId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyMealBreakfast_DailyMealId",
                table: "DailyMealBreakfast",
                column: "DailyMealId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMealDinner_DinnerId",
                table: "DailyMealDinner",
                column: "DinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMealLunch_LunchId",
                table: "DailyMealLunch",
                column: "LunchId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMeals_MealPlanId",
                table: "DailyMeals",
                column: "MealPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMealSnacks_SnacksId",
                table: "DailyMealSnacks",
                column: "SnacksId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyMealBreakfast");

            migrationBuilder.DropTable(
                name: "DailyMealDinner");

            migrationBuilder.DropTable(
                name: "DailyMealLunch");

            migrationBuilder.DropTable(
                name: "DailyMealSnacks");

            migrationBuilder.DropTable(
                name: "DailyMeals");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "MealPlans");
        }
    }
}
