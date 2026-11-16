using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ayloul.Migrations
{
    /// <inheritdoc />
    public partial class addService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_RoseColors_RoseColor_Id",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "RoseColor_Id",
                table: "Bookings",
                newName: "RoseColor1_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_RoseColor_Id",
                table: "Bookings",
                newName: "IX_Bookings_RoseColor1_Id");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Locations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Bookings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BasePrice",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Event_Date",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Bookings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note_Image",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone_Number",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RoseColor2_Id",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Service_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Service_Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Service_Id);
                });

            migrationBuilder.CreateTable(
                name: "BookingServices",
                columns: table => new
                {
                    BookingService_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Booking_Id = table.Column<int>(type: "int", nullable: false),
                    Service_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingServices", x => x.BookingService_Id);
                    table.ForeignKey(
                        name: "FK_BookingServices_Bookings_Booking_Id",
                        column: x => x.Booking_Id,
                        principalTable: "Bookings",
                        principalColumn: "Booking_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingServices_Services_Service_Id",
                        column: x => x.Service_Id,
                        principalTable: "Services",
                        principalColumn: "Service_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoseColor2_Id",
                table: "Bookings",
                column: "RoseColor2_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookingServices_Booking_Id",
                table: "BookingServices",
                column: "Booking_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookingServices_Service_Id",
                table: "BookingServices",
                column: "Service_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_RoseColors_RoseColor1_Id",
                table: "Bookings",
                column: "RoseColor1_Id",
                principalTable: "RoseColors",
                principalColumn: "RoseColor_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_RoseColors_RoseColor2_Id",
                table: "Bookings",
                column: "RoseColor2_Id",
                principalTable: "RoseColors",
                principalColumn: "RoseColor_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_RoseColors_RoseColor1_Id",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_RoseColors_RoseColor2_Id",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "BookingServices");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RoseColor2_Id",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Event_Date",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Note_Image",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Phone_Number",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RoseColor2_Id",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "RoseColor1_Id",
                table: "Bookings",
                newName: "RoseColor_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_RoseColor1_Id",
                table: "Bookings",
                newName: "IX_Bookings_RoseColor_Id");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Bookings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_RoseColors_RoseColor_Id",
                table: "Bookings",
                column: "RoseColor_Id",
                principalTable: "RoseColors",
                principalColumn: "RoseColor_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
