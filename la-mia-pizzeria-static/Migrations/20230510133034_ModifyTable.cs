using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace la_mia_pizzeria_static.Migrations
{
    /// <inheritdoc />
    public partial class ModifyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientPizza_ingredients_IngredientId",
                table: "IngredientPizza");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "pizzas");

            migrationBuilder.RenameColumn(
                name: "IngredientId",
                table: "IngredientPizza",
                newName: "IngredientsId");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientPizza_ingredients_IngredientsId",
                table: "IngredientPizza",
                column: "IngredientsId",
                principalTable: "ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientPizza_ingredients_IngredientsId",
                table: "IngredientPizza");

            migrationBuilder.RenameColumn(
                name: "IngredientsId",
                table: "IngredientPizza",
                newName: "IngredientId");

            migrationBuilder.AddColumn<int>(
                name: "IngredientId",
                table: "pizzas",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientPizza_ingredients_IngredientId",
                table: "IngredientPizza",
                column: "IngredientId",
                principalTable: "ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
