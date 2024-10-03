using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiztle.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class migration0009 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestsQuestions_Questions_QuestionId",
                table: "TestsQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_TestsQuestions_Tests_TestId",
                table: "TestsQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestsQuestions",
                table: "TestsQuestions");

            migrationBuilder.RenameTable(
                name: "TestsQuestions",
                newName: "TestQuestion");

            migrationBuilder.RenameIndex(
                name: "IX_TestsQuestions_QuestionId",
                table: "TestQuestion",
                newName: "IX_TestQuestion_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestQuestion",
                table: "TestQuestion",
                columns: new[] { "TestId", "QuestionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestion_Questions_QuestionId",
                table: "TestQuestion",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestQuestion_Tests_TestId",
                table: "TestQuestion",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestion_Questions_QuestionId",
                table: "TestQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_TestQuestion_Tests_TestId",
                table: "TestQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestQuestion",
                table: "TestQuestion");

            migrationBuilder.RenameTable(
                name: "TestQuestion",
                newName: "TestsQuestions");

            migrationBuilder.RenameIndex(
                name: "IX_TestQuestion_QuestionId",
                table: "TestsQuestions",
                newName: "IX_TestsQuestions_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestsQuestions",
                table: "TestsQuestions",
                columns: new[] { "TestId", "QuestionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TestsQuestions_Questions_QuestionId",
                table: "TestsQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestsQuestions_Tests_TestId",
                table: "TestsQuestions",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
