using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ayloul.Migrations
{
    /// <inheritdoc />
    public partial class addbackground : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Backgrounds_Background_Id",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "Background_Id",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Upload_Background",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Backgrounds_Background_Id",
                table: "Bookings",
                column: "Background_Id",
                principalTable: "Backgrounds",
                principalColumn: "Background_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Backgrounds_Background_Id",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Upload_Background",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "Background_Id",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Backgrounds_Background_Id",
                table: "Bookings",
                column: "Background_Id",
                principalTable: "Backgrounds",
                principalColumn: "Background_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
