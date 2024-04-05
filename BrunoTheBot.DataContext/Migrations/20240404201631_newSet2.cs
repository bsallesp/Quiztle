using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class newSet2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Topics_TopicClassId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Topics_TopicId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_References_Topics_TopicClassId",
                table: "References");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Schools_SchoolId",
                table: "Topics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topics",
                table: "Topics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_References",
                table: "References");

            migrationBuilder.RenameTable(
                name: "Topics",
                newName: "TopicClasses");

            migrationBuilder.RenameTable(
                name: "References",
                newName: "Places");

            migrationBuilder.RenameIndex(
                name: "IX_Topics_SchoolId",
                table: "TopicClasses",
                newName: "IX_TopicClasses_SchoolId");

            migrationBuilder.RenameIndex(
                name: "IX_References_TopicClassId",
                table: "Places",
                newName: "IX_Places_TopicClassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicClasses",
                table: "TopicClasses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Places",
                table: "Places",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_TopicClasses_TopicClassId",
                table: "Authors",
                column: "TopicClassId",
                principalTable: "TopicClasses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_TopicClasses_TopicClassId",
                table: "Places",
                column: "TopicClassId",
                principalTable: "TopicClasses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_TopicClasses_TopicId",
                table: "Questions",
                column: "TopicId",
                principalTable: "TopicClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicClasses_Schools_SchoolId",
                table: "TopicClasses",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_TopicClasses_TopicClassId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Places_TopicClasses_TopicClassId",
                table: "Places");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_TopicClasses_TopicId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicClasses_Schools_SchoolId",
                table: "TopicClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicClasses",
                table: "TopicClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Places",
                table: "Places");

            migrationBuilder.RenameTable(
                name: "TopicClasses",
                newName: "Topics");

            migrationBuilder.RenameTable(
                name: "Places",
                newName: "References");

            migrationBuilder.RenameIndex(
                name: "IX_TopicClasses_SchoolId",
                table: "Topics",
                newName: "IX_Topics_SchoolId");

            migrationBuilder.RenameIndex(
                name: "IX_Places_TopicClassId",
                table: "References",
                newName: "IX_References_TopicClassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topics",
                table: "Topics",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_References",
                table: "References",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Topics_TopicClassId",
                table: "Authors",
                column: "TopicClassId",
                principalTable: "Topics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Topics_TopicId",
                table: "Questions",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_References_Topics_TopicClassId",
                table: "References",
                column: "TopicClassId",
                principalTable: "Topics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Schools_SchoolId",
                table: "Topics",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id");
        }
    }
}
