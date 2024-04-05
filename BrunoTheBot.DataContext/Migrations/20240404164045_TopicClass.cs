using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class TopicClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_References_Topics_TopicId",
                table: "References");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "References",
                newName: "TopicClassId");

            migrationBuilder.RenameIndex(
                name: "IX_References_TopicId",
                table: "References",
                newName: "IX_References_TopicClassId");

            migrationBuilder.AddColumn<int>(
                name: "TopicClassId",
                table: "Authors",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_TopicClassId",
                table: "Authors",
                column: "TopicClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Topics_TopicClassId",
                table: "Authors",
                column: "TopicClassId",
                principalTable: "Topics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_References_Topics_TopicClassId",
                table: "References",
                column: "TopicClassId",
                principalTable: "Topics",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Topics_TopicClassId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_References_Topics_TopicClassId",
                table: "References");

            migrationBuilder.DropIndex(
                name: "IX_Authors_TopicClassId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "TopicClassId",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "TopicClassId",
                table: "References",
                newName: "TopicId");

            migrationBuilder.RenameIndex(
                name: "IX_References_TopicClassId",
                table: "References",
                newName: "IX_References_TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_References_Topics_TopicId",
                table: "References",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id");
        }
    }
}
