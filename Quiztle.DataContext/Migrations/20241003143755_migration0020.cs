using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiztle.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class migration0020 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookTasks_Users_UserId",
                table: "BookTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_BookTasks_Users_UserId",
                table: "BookTasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookTasks_Users_UserId",
                table: "BookTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_BookTasks_Users_UserId",
                table: "BookTasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
