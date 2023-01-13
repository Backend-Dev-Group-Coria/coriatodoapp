using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoriaToDo.API.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Order",
                table: "ToDoItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "ToDoItems");
        }
    }
}
