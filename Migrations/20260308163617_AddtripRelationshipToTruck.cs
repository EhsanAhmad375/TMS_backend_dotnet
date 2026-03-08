using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Migrations
{
    /// <inheritdoc />
    public partial class AddtripRelationshipToTruck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fuel_type",
                table: "trucks",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "tripId",
                table: "trucks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_trucks_tripId",
                table: "trucks",
                column: "tripId");

            migrationBuilder.AddForeignKey(
                name: "FK_trucks_trips_tripId",
                table: "trucks",
                column: "tripId",
                principalTable: "trips",
                principalColumn: "tripId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_trucks_trips_tripId",
                table: "trucks");

            migrationBuilder.DropIndex(
                name: "IX_trucks_tripId",
                table: "trucks");

            migrationBuilder.DropColumn(
                name: "fuel_type",
                table: "trucks");

            migrationBuilder.DropColumn(
                name: "tripId",
                table: "trucks");
        }
    }
}
