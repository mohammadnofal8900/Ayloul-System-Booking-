using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ayloul.Migrations
{
    /// <inheritdoc />
    public partial class addpaymentmethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_Booking_Id",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "paymentMethod",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Booking_Id",
                table: "Payments",
                column: "Booking_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_Booking_Id",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "paymentMethod",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Booking_Id",
                table: "Payments",
                column: "Booking_Id",
                unique: true);
        }
    }
}
