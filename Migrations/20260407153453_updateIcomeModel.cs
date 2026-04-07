using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Migrations
{
    /// <inheritdoc />
    public partial class updateIcomeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amount",
                table: "Incomes");

            migrationBuilder.AddColumn<double>(
                name: "remaining_amount",
                table: "Incomes",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "reveived_amount",
                table: "Incomes",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "total_amount",
                table: "Incomes",
                type: "double",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "remaining_amount",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "reveived_amount",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "total_amount",
                table: "Incomes");

            migrationBuilder.AddColumn<double>(
                name: "amount",
                table: "Incomes",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
