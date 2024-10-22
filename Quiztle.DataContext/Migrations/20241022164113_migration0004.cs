using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiztle.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class migration0004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestsPerformance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TestName = table.Column<string>(type: "text", nullable: false),
                    CorrectAnswers = table.Column<int>(type: "integer", nullable: false),
                    IncorrectAnswers = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestsPerformance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionsPerformance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionName = table.Column<string>(type: "text", nullable: false),
                    CorrectAnswerName = table.Column<string>(type: "text", nullable: false),
                    IncorrectAnswerName = table.Column<string>(type: "text", nullable: false),
                    TagName = table.Column<string>(type: "text", nullable: false),
                    TestPerformanceId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsPerformance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionsPerformance_TestsPerformance_TestPerformanceId",
                        column: x => x.TestPerformanceId,
                        principalTable: "TestsPerformance",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsPerformance_TestPerformanceId",
                table: "QuestionsPerformance",
                column: "TestPerformanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionsPerformance");

            migrationBuilder.DropTable(
                name: "TestsPerformance");
        }
    }
}
