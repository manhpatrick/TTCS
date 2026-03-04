using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixroombooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PricePerMonth",
                table: "Room",
                newName: "PricePerNight");

            migrationBuilder.AddColumn<decimal>(
                name: "AverageStar",
                table: "Room",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BookingCount",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Room",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalRatingCount",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageStar",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "BookingCount",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "TotalRatingCount",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "PricePerNight",
                table: "Room",
                newName: "PricePerMonth");
        }
    }
}
