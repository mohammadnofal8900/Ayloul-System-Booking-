using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ayloul.Migrations
{
    /// <inheritdoc />
    public partial class deletecostumerOrderPhotos2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostumerOrderPhoto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostumerOrderPhoto",
                columns: table => new
                {
                    CostumerOrderPhoto_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Booking_Id1 = table.Column<int>(type: "int", nullable: true),
                    Booking_Id = table.Column<int>(type: "int", nullable: false),
                    Fill_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Fill_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fill_Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fill_Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fill_Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostumerOrderPhoto", x => x.CostumerOrderPhoto_Id);
                    table.ForeignKey(
                        name: "FK_CostumerOrderPhoto_Bookings_Booking_Id1",
                        column: x => x.Booking_Id1,
                        principalTable: "Bookings",
                        principalColumn: "Booking_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostumerOrderPhoto_Booking_Id1",
                table: "CostumerOrderPhoto",
                column: "Booking_Id1");
        }
    }
}
