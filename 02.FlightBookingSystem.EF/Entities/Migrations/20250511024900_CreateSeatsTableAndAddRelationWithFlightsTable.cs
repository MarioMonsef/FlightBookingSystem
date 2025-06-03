using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _02.FlightBookingSystem.EF.Entities.Migrations
{
    /// <inheritdoc />
    public partial class CreateSeatsTableAndAddRelationWithFlightsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeatNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsBooking = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FlightID = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.ID);
                    table.CheckConstraint("CK_Seat_SeatNumber_Format", "SeatNumber LIKE '[A-Z][0-9][0-9]'");
                    table.ForeignKey(
                        name: "FK_Seats_flights_FlightID",
                        column: x => x.FlightID,
                        principalTable: "flights",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_FlightID",
                table: "Seats",
                column: "FlightID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seats");
        }
    }
}
