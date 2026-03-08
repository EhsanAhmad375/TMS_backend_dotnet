using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Migrations
{
    /// <inheritdoc />
    public partial class InitialBusinessModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "expenseCategories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expenseCategories", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "trucks",
                columns: table => new
                {
                    truckId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    plate_number = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    model = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    make = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    year = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sub_status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fuel_percentage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mileage_km = table.Column<int>(type: "int", nullable: true),
                    tire_condition = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    engine_capacity = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    max_load_tons = table.Column<int>(type: "int", nullable: true),
                    rating = table.Column<double>(type: "double", nullable: true),
                    image_url = table.Column<double>(type: "double", nullable: true),
                    driver_id = table.Column<int>(type: "int", nullable: true),
                    co_driver_id = table.Column<int>(type: "int", nullable: true),
                    registration_expiry = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    insurance_type = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    permit_type = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fitness_cert_expiry = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cnic_status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trucks", x => x.truckId);
                    table.ForeignKey(
                        name: "FK_trucks_users_co_driver_id",
                        column: x => x.co_driver_id,
                        principalTable: "users",
                        principalColumn: "userId");
                    table.ForeignKey(
                        name: "FK_trucks_users_driver_id",
                        column: x => x.driver_id,
                        principalTable: "users",
                        principalColumn: "userId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "trips",
                columns: table => new
                {
                    tripId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    trip_number = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    truck_id = table.Column<int>(type: "int", nullable: true),
                    driver_id = table.Column<int>(type: "int", nullable: true),
                    co_driver_id = table.Column<int>(type: "int", nullable: true),
                    client_Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    client_contact = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    client_company = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    trip_type = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pickup_location = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    destination = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    distance_km = table.Column<double>(type: "double", nullable: true),
                    estimated_time_min = table.Column<int>(type: "int", nullable: true),
                    allowance = table.Column<double>(type: "double", nullable: true),
                    actual_start_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    actual_end_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    odometer_start = table.Column<int>(type: "int", nullable: true),
                    odometer_end = table.Column<int>(type: "int", nullable: true),
                    notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    scheduled_date = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trips", x => x.tripId);
                    table.ForeignKey(
                        name: "FK_trips_trucks_truck_id",
                        column: x => x.truck_id,
                        principalTable: "trucks",
                        principalColumn: "truckId");
                    table.ForeignKey(
                        name: "FK_trips_users_co_driver_id",
                        column: x => x.co_driver_id,
                        principalTable: "users",
                        principalColumn: "userId");
                    table.ForeignKey(
                        name: "FK_trips_users_driver_id",
                        column: x => x.driver_id,
                        principalTable: "users",
                        principalColumn: "userId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "expenses",
                columns: table => new
                {
                    expenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    trip_id = table.Column<int>(type: "int", nullable: true),
                    driver_id = table.Column<int>(type: "int", nullable: true),
                    co_driver_id = table.Column<int>(type: "int", nullable: true),
                    client_Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    e_c_id = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<double>(type: "double", nullable: true),
                    date = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    time = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    location = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    receipt_url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    added_by = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expenses", x => x.expenseId);
                    table.ForeignKey(
                        name: "FK_expenses_expenseCategories_e_c_id",
                        column: x => x.e_c_id,
                        principalTable: "expenseCategories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_expenses_trips_trip_id",
                        column: x => x.trip_id,
                        principalTable: "trips",
                        principalColumn: "tripId");
                    table.ForeignKey(
                        name: "FK_expenses_users_added_by",
                        column: x => x.added_by,
                        principalTable: "users",
                        principalColumn: "userId");
                    table.ForeignKey(
                        name: "FK_expenses_users_co_driver_id",
                        column: x => x.co_driver_id,
                        principalTable: "users",
                        principalColumn: "userId");
                    table.ForeignKey(
                        name: "FK_expenses_users_driver_id",
                        column: x => x.driver_id,
                        principalTable: "users",
                        principalColumn: "userId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tripStatuses",
                columns: table => new
                {
                    tripStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    statusName = table.Column<int>(type: "int", nullable: false),
                    tripId = table.Column<int>(type: "int", nullable: false),
                    truckId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tripStatuses", x => x.tripStatusId);
                    table.ForeignKey(
                        name: "FK_tripStatuses_trips_tripId",
                        column: x => x.tripId,
                        principalTable: "trips",
                        principalColumn: "tripId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tripStatuses_trucks_truckId",
                        column: x => x.truckId,
                        principalTable: "trucks",
                        principalColumn: "truckId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_added_by",
                table: "expenses",
                column: "added_by");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_co_driver_id",
                table: "expenses",
                column: "co_driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_driver_id",
                table: "expenses",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_e_c_id",
                table: "expenses",
                column: "e_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_trip_id",
                table: "expenses",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "IX_trips_co_driver_id",
                table: "trips",
                column: "co_driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_trips_driver_id",
                table: "trips",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_trips_truck_id",
                table: "trips",
                column: "truck_id");

            migrationBuilder.CreateIndex(
                name: "IX_tripStatuses_tripId",
                table: "tripStatuses",
                column: "tripId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tripStatuses_truckId",
                table: "tripStatuses",
                column: "truckId");

            migrationBuilder.CreateIndex(
                name: "IX_trucks_co_driver_id",
                table: "trucks",
                column: "co_driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_trucks_driver_id",
                table: "trucks",
                column: "driver_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "expenses");

            migrationBuilder.DropTable(
                name: "tripStatuses");

            migrationBuilder.DropTable(
                name: "expenseCategories");

            migrationBuilder.DropTable(
                name: "trips");

            migrationBuilder.DropTable(
                name: "trucks");
        }
    }
}
