using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ayloul.Migrations
{
    /// <inheritdoc />
    public partial class AddChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    ChatRoom_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Employee_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.ChatRoom_Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Message_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatRoom_Id = table.Column<int>(type: "int", nullable: false),
                    ChatRoom_Id1 = table.Column<int>(type: "int", nullable: false),
                    Sender_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message_Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Message_Id);
                    table.ForeignKey(
                        name: "FK_Messages_ChatRooms_ChatRoom_Id1",
                        column: x => x.ChatRoom_Id1,
                        principalTable: "ChatRooms",
                        principalColumn: "ChatRoom_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatRoom_Id1",
                table: "Messages",
                column: "ChatRoom_Id1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ChatRooms");
        }
    }
}
