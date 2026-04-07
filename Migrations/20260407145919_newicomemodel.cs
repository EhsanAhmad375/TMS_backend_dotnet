using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Migrations
{
    /// <inheritdoc />
    public partial class newicomemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomeSources",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeSources", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    incomeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    trip_id = table.Column<int>(type: "int", nullable: true),
                    income_source_id = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<double>(type: "double", nullable: false),
                    date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    added_by = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.incomeId);
                    table.ForeignKey(
                        name: "FK_Incomes_IncomeSources_income_source_id",
                        column: x => x.income_source_id,
                        principalTable: "IncomeSources",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Incomes_trips_trip_id",
                        column: x => x.trip_id,
                        principalTable: "trips",
                        principalColumn: "tripId");
                    table.ForeignKey(
                        name: "FK_Incomes_users_added_by",
                        column: x => x.added_by,
                        principalTable: "users",
                        principalColumn: "userId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_added_by",
                table: "Incomes",
                column: "added_by");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_income_source_id",
                table: "Incomes",
                column: "income_source_id");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_trip_id",
                table: "Incomes",
                column: "trip_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.DropTable(
                name: "IncomeSources");
        }
    }
}
