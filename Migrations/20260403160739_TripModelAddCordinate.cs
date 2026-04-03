using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Migrations
{
    /// <inheritdoc />
    public partial class TripModelAddCordinate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "curr_lat",
                table: "trips",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "curr_lng",
                table: "trips",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "des_lat",
                table: "trips",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "des_lng",
                table: "trips",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "pic_lat",
                table: "trips",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "pic_lng",
                table: "trips",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "curr_lat",
                table: "trips");

            migrationBuilder.DropColumn(
                name: "curr_lng",
                table: "trips");

            migrationBuilder.DropColumn(
                name: "des_lat",
                table: "trips");

            migrationBuilder.DropColumn(
                name: "des_lng",
                table: "trips");

            migrationBuilder.DropColumn(
                name: "pic_lat",
                table: "trips");

            migrationBuilder.DropColumn(
                name: "pic_lng",
                table: "trips");
        }
    }
}
