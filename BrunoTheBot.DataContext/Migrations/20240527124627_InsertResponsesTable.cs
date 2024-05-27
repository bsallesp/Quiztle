using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class InsertResponsesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ResponseId",
                table: "Questions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responses_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ResponseId",
                table: "Questions",
                column: "ResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_TestId",
                table: "Responses",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Responses_ResponseId",
                table: "Questions",
                column: "ResponseId",
                principalTable: "Responses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Responses_ResponseId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "Responses");

            migrationBuilder.DropIndex(
                name: "IX_Questions_ResponseId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ResponseId",
                table: "Questions");
        }
    }
}
