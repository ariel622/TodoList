using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTodoItemStatusToIncomplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "TodoItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsIncomplete",
                table: "TodoItems",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIncomplete",
                table: "TodoItems");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "TodoItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Pending");
        }
    }
}
