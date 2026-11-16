using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ayloul.Migrations
{
    /// <inheritdoc />
    public partial class m5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<string>(
                name: "img",
                table: "RoseTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "img",
                table: "RoseColors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "img",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "img",
                table: "FrameColors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Booking_Number",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "img",
                table: "Backgrounds",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "img",
                table: "RoseTypes");

            migrationBuilder.DropColumn(
                name: "img",
                table: "RoseColors");

            migrationBuilder.DropColumn(
                name: "img",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "img",
                table: "FrameColors");

            migrationBuilder.DropColumn(
                name: "Booking_Number",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "img",
                table: "Backgrounds");

           
        }
    }
}
