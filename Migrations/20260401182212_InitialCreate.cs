using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Group6Flight.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airline",
                columns: table => new
                {
                    AirlineId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImageName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airline", x => x.AirlineId);
                });

            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightCode = table.Column<string>(type: "TEXT", nullable: false),
                    From = table.Column<string>(type: "TEXT", nullable: false),
                    To = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DepartureTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    CabinType = table.Column<string>(type: "TEXT", nullable: false),
                    Emission = table.Column<double>(type: "REAL", nullable: false),
                    AircraftType = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    AirlineId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => x.FlightId);
                    table.ForeignKey(
                        name: "FK_Flight_Airline_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airline",
                        principalColumn: "AirlineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightBookings",
                columns: table => new
                {
                    FlightBookingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightBookings", x => x.FlightBookingId);
                    table.ForeignKey(
                        name: "FK_FlightBookings_Flight_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flight",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Airline",
                columns: new[] { "AirlineId", "ImageName", "Name" },
                values: new object[,]
                {
                    { 1, "american_airlines.png", "American Airlines" },
                    { 2, "delta_air_lines.png", "Delta Air Lines" },
                    { 3, "united_airlines.png", "United Airlines" },
                    { 4, "southwest_airlines.png", "Southwest Airlines" },
                    { 5, "alaska_airlines.png", "Alaska Airlines" },
                    { 6, "jetblue_airways.png", "JetBlue Airways" }
                });

            migrationBuilder.InsertData(
                table: "Flight",
                columns: new[] { "FlightId", "AircraftType", "AirlineId", "ArrivalTime", "CabinType", "Date", "DepartureTime", "Emission", "FlightCode", "From", "Price", "To" },
                values: new object[,]
                {
                    { 1, "Boeing 737", 1, new TimeSpan(0, 11, 45, 0, 0), "Economy", new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 30, 0, 0), 250.5, "AA101", "New York", 320.00m, "Los Angeles" },
                    { 2, "Airbus A320", 2, new TimeSpan(0, 16, 10, 0, 0), "Business", new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 14, 15, 0, 0), 180.19999999999999, "DL202", "Atlanta", 450.00m, "Chicago" },
                    { 3, "Boeing 757", 3, new TimeSpan(0, 11, 0, 0, 0), "Economy", new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0), 120.0, "UA303", "San Francisco", 210.00m, "Seattle" },
                    { 4, "Boeing 737", 4, new TimeSpan(0, 8, 30, 0, 0), "Economy", new DateTime(2026, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 6, 45, 0, 0), 140.69999999999999, "SW404", "Dallas", 180.00m, "Denver" },
                    { 5, "Boeing 777", 5, new TimeSpan(0, 10, 0, 0, 0), "First Class", new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 22, 0, 0, 0), 500.0, "BA505", "Boston", 1200.00m, "London" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flight_AirlineId",
                table: "Flight",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightBookings_FlightId",
                table: "FlightBookings",
                column: "FlightId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightBookings");

            migrationBuilder.DropTable(
                name: "Flight");

            migrationBuilder.DropTable(
                name: "Airline");
        }
    }
}
