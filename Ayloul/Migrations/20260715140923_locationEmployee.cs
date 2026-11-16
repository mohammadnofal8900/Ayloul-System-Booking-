using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ayloul.Migrations
{
    /// <inheritdoc />
    public partial class locationEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Location_Id",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Location_Id",
                table: "Employees",
                column: "Location_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Locations_Location_Id",
                table: "Employees",
                column: "Location_Id",
                principalTable: "Locations",
                principalColumn: "Location_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Locations_Location_Id",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Location_Id",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Location_Id",
                table: "Employees");
        }
    }
}
