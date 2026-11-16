using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ayloul.Migrations
{
    /// <inheritdoc />
    public partial class chat2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatRooms_ChatRoom_Id1",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatRoom_Id1",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChatRoom_Id1",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "Sender_Id",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Employee_Id",
                table: "ChatRooms",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Customer_Id",
                table: "ChatRooms",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatRoom_Id",
                table: "Messages",
                column: "ChatRoom_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_Sender_Id",
                table: "Messages",
                column: "Sender_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_Customer_Id",
                table: "ChatRooms",
                column: "Customer_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_Employee_Id",
                table: "ChatRooms",
                column: "Employee_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRooms_AspNetUsers_Customer_Id",
                table: "ChatRooms",
                column: "Customer_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRooms_AspNetUsers_Employee_Id",
                table: "ChatRooms",
                column: "Employee_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_Sender_Id",
                table: "Messages",
                column: "Sender_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatRooms_ChatRoom_Id",
                table: "Messages",
                column: "ChatRoom_Id",
                principalTable: "ChatRooms",
                principalColumn: "ChatRoom_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRooms_AspNetUsers_Customer_Id",
                table: "ChatRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatRooms_AspNetUsers_Employee_Id",
                table: "ChatRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_Sender_Id",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatRooms_ChatRoom_Id",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatRoom_Id",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_Sender_Id",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_ChatRooms_Customer_Id",
                table: "ChatRooms");

            migrationBuilder.DropIndex(
                name: "IX_ChatRooms_Employee_Id",
                table: "ChatRooms");

            migrationBuilder.AlterColumn<string>(
                name: "Sender_Id",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ChatRoom_Id1",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Employee_Id",
                table: "ChatRooms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Customer_Id",
                table: "ChatRooms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatRoom_Id1",
                table: "Messages",
                column: "ChatRoom_Id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatRooms_ChatRoom_Id1",
                table: "Messages",
                column: "ChatRoom_Id1",
                principalTable: "ChatRooms",
                principalColumn: "ChatRoom_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
