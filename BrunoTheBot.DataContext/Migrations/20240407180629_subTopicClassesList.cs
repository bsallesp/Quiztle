using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class subTopicClassesList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicClasses_TopicClasses_SubTopicClassId",
                table: "TopicClasses");

            migrationBuilder.DropIndex(
                name: "IX_TopicClasses_SubTopicClassId",
                table: "TopicClasses");

            migrationBuilder.DropColumn(
                name: "SubTopicClassId",
                table: "TopicClasses");

            migrationBuilder.AddColumn<int>(
                name: "TopicClassId",
                table: "TopicClasses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopicClasses_TopicClassId",
                table: "TopicClasses",
                column: "TopicClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicClasses_TopicClasses_TopicClassId",
                table: "TopicClasses",
                column: "TopicClassId",
                principalTable: "TopicClasses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicClasses_TopicClasses_TopicClassId",
                table: "TopicClasses");

            migrationBuilder.DropIndex(
                name: "IX_TopicClasses_TopicClassId",
                table: "TopicClasses");

            migrationBuilder.DropColumn(
                name: "TopicClassId",
                table: "TopicClasses");

            migrationBuilder.AddColumn<int>(
                name: "SubTopicClassId",
                table: "TopicClasses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TopicClasses_SubTopicClassId",
                table: "TopicClasses",
                column: "SubTopicClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicClasses_TopicClasses_SubTopicClassId",
                table: "TopicClasses",
                column: "SubTopicClassId",
                principalTable: "TopicClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
