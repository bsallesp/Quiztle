using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class subTopicClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
