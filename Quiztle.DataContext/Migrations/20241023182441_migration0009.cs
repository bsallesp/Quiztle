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
                name: "FK_QuestionsPerformance_TestsPerformance_TestPerformanceId",
                table: "QuestionsPerformance");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsPerformance_TestsPerformance_TestPerformanceId",
                table: "QuestionsPerformance",
                column: "TestPerformanceId",
                principalTable: "TestsPerformance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsPerformance_TestsPerformance_TestPerformanceId",
                table: "QuestionsPerformance");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsPerformance_TestsPerformance_TestPerformanceId",
                table: "QuestionsPerformance",
                column: "TestPerformanceId",
                principalTable: "TestsPerformance",
                principalColumn: "Id");
        }
    }
}
