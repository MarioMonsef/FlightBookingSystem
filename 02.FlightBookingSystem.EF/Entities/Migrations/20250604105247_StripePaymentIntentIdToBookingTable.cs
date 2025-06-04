using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _02.FlightBookingSystem.EF.Entities.Migrations
{
    /// <inheritdoc />
    public partial class StripePaymentIntentIdToBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingDate",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 6, 4, 10, 52, 46, 720, DateTimeKind.Utc).AddTicks(2681),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 5, 11, 3, 7, 51, 435, DateTimeKind.Utc).AddTicks(2159));

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "Bookings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingDate",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 5, 11, 3, 7, 51, 435, DateTimeKind.Utc).AddTicks(2159),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 6, 4, 10, 52, 46, 720, DateTimeKind.Utc).AddTicks(2681));
        }
    }
}
