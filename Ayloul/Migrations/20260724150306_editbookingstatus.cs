using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ayloul.Migrations
{
    /// <inheritdoc />
    public partial class editbookingstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "bookingStatus",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bookingStatus",
                table: "Bookings");
        }
    }
}
