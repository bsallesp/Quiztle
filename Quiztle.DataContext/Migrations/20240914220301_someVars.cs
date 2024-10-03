using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiztle.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class someVars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Drafts_DraftId",
                table: "Questions");

            migrationBuilder.AddColumn<Guid>(
                name: "DraftId1",
                table: "Questions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_DraftId1",
                table: "Questions",
                column: "DraftId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Drafts_DraftId",
                table: "Questions",
                column: "DraftId",
                principalTable: "Drafts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Drafts_DraftId1",
                table: "Questions",
                column: "DraftId1",
                principalTable: "Drafts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Drafts_DraftId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Drafts_DraftId1",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_DraftId1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "DraftId1",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Drafts_DraftId",
                table: "Questions",
                column: "DraftId",
                principalTable: "Drafts",
                principalColumn: "Id");
        }
    }
}
