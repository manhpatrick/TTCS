using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addfieldroomimage1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsThumbnail",
                table: "RoomImage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "RoomImage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsThumbnail",
                table: "RoomImage");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "RoomImage");
        }
    }
}
