using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat_Application.Migrations
{
    /// <inheritdoc />
    public partial class appendUserMessagestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMessage_AspNetUsers_RecepientUser_Id",
                table: "UserMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMessage_AspNetUsers_SenderUser_Id",
                table: "UserMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMessage",
                table: "UserMessage");

            migrationBuilder.RenameTable(
                name: "UserMessage",
                newName: "UserMessages");

            migrationBuilder.RenameIndex(
                name: "IX_UserMessage_SenderUser_Id",
                table: "UserMessages",
                newName: "IX_UserMessages_SenderUser_Id");

            migrationBuilder.RenameIndex(
                name: "IX_UserMessage_RecepientUser_Id",
                table: "UserMessages",
                newName: "IX_UserMessages_RecepientUser_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMessages",
                table: "UserMessages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessages_AspNetUsers_RecepientUser_Id",
                table: "UserMessages",
                column: "RecepientUser_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessages_AspNetUsers_SenderUser_Id",
                table: "UserMessages",
                column: "SenderUser_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMessages_AspNetUsers_RecepientUser_Id",
                table: "UserMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMessages_AspNetUsers_SenderUser_Id",
                table: "UserMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMessages",
                table: "UserMessages");

            migrationBuilder.RenameTable(
                name: "UserMessages",
                newName: "UserMessage");

            migrationBuilder.RenameIndex(
                name: "IX_UserMessages_SenderUser_Id",
                table: "UserMessage",
                newName: "IX_UserMessage_SenderUser_Id");

            migrationBuilder.RenameIndex(
                name: "IX_UserMessages_RecepientUser_Id",
                table: "UserMessage",
                newName: "IX_UserMessage_RecepientUser_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMessage",
                table: "UserMessage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessage_AspNetUsers_RecepientUser_Id",
                table: "UserMessage",
                column: "RecepientUser_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessage_AspNetUsers_SenderUser_Id",
                table: "UserMessage",
                column: "SenderUser_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
