using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Migrations
{
    /// <inheritdoc />
    public partial class AddExpLatAndLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "exp_lat",
                table: "expenses",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "exp_long",
                table: "expenses",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "exp_lat",
                table: "expenses");

            migrationBuilder.DropColumn(
                name: "exp_long",
                table: "expenses");
        }
    }
}
