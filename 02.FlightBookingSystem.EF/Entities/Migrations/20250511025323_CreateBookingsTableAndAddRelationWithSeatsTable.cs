using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _02.FlightBookingSystem.EF.Entities.Migrations
{
    /// <inheritdoc />
    public partial class CreateBookingsTableAndAddRelationWithSeatsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2025, 5, 11, 2, 53, 23, 381, DateTimeKind.Utc).AddTicks(4205)),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SeatID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Bookings_Seats_SeatID",
                        column: x => x.SeatID,
                        principalTable: "Seats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SeatID",
                table: "Bookings",
                column: "SeatID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
