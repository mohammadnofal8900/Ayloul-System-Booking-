using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ayloul.Migrations
{
    /// <inheritdoc />
    public partial class deletecostumerOrderPhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_costumerOrderPhotos_Bookings_Booking_Id1",
                table: "costumerOrderPhotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_costumerOrderPhotos",
                table: "costumerOrderPhotos");

            migrationBuilder.RenameTable(
                name: "costumerOrderPhotos",
                newName: "CostumerOrderPhoto");

            migrationBuilder.RenameIndex(
                name: "IX_costumerOrderPhotos_Booking_Id1",
                table: "CostumerOrderPhoto",
                newName: "IX_CostumerOrderPhoto_Booking_Id1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CostumerOrderPhoto",
                table: "CostumerOrderPhoto",
                column: "CostumerOrderPhoto_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CostumerOrderPhoto_Bookings_Booking_Id1",
                table: "CostumerOrderPhoto",
                column: "Booking_Id1",
                principalTable: "Bookings",
                principalColumn: "Booking_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostumerOrderPhoto_Bookings_Booking_Id1",
                table: "CostumerOrderPhoto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CostumerOrderPhoto",
                table: "CostumerOrderPhoto");

            migrationBuilder.RenameTable(
                name: "CostumerOrderPhoto",
                newName: "costumerOrderPhotos");

            migrationBuilder.RenameIndex(
                name: "IX_CostumerOrderPhoto_Booking_Id1",
                table: "costumerOrderPhotos",
                newName: "IX_costumerOrderPhotos_Booking_Id1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_costumerOrderPhotos",
                table: "costumerOrderPhotos",
                column: "CostumerOrderPhoto_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_costumerOrderPhotos_Bookings_Booking_Id1",
                table: "costumerOrderPhotos",
                column: "Booking_Id1",
                principalTable: "Bookings",
                principalColumn: "Booking_Id");
        }
    }
}
