using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ayloul.Migrations
{
    /// <inheritdoc />
    public partial class m3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auditDeleteBookings");

            migrationBuilder.CreateTable(
                name: "auditBookings",
                columns: table => new
                {
                    AuditBooking_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Booking_Id = table.Column<int>(type: "int", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    User_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Delete_Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auditBookings", x => x.AuditBooking_Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auditBookings");

            migrationBuilder.CreateTable(
                name: "auditDeleteBookings",
                columns: table => new
                {
                    AuditDeleteBooking_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Booking_Id = table.Column<int>(type: "int", nullable: false),
                    Delete_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auditDeleteBookings", x => x.AuditDeleteBooking_Id);
                });
        }
    }
}
