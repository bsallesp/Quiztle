using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class placesFromTopicsToSchool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_TopicClasses_TopicClassId",
                table: "Places");

            migrationBuilder.RenameColumn(
                name: "TopicClassId",
                table: "Places",
                newName: "SchoolId");

            migrationBuilder.RenameIndex(
                name: "IX_Places_TopicClassId",
                table: "Places",
                newName: "IX_Places_SchoolId");

            migrationBuilder.AddColumn<int>(
                name: "ContentId",
                table: "Places",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Places_ContentId",
                table: "Places",
                column: "ContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Contents_ContentId",
                table: "Places",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Schools_SchoolId",
                table: "Places",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_Contents_ContentId",
                table: "Places");

            migrationBuilder.DropForeignKey(
                name: "FK_Places_Schools_SchoolId",
                table: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Places_ContentId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "Places");

            migrationBuilder.RenameColumn(
                name: "SchoolId",
                table: "Places",
                newName: "TopicClassId");

            migrationBuilder.RenameIndex(
                name: "IX_Places_SchoolId",
                table: "Places",
                newName: "IX_Places_TopicClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_TopicClasses_TopicClassId",
                table: "Places",
                column: "TopicClassId",
                principalTable: "TopicClasses",
                principalColumn: "Id");
        }
    }
}
