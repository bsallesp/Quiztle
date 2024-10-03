using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiztle.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class DraftAndScratchTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Drafts",
                table: "Scratches");

            migrationBuilder.AddColumn<Guid>(
                name: "DraftId",
                table: "Questions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Drafts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    ScratchId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drafts_Scratches_ScratchId",
                        column: x => x.ScratchId,
                        principalTable: "Scratches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_DraftId",
                table: "Questions",
                column: "DraftId");

            migrationBuilder.CreateIndex(
                name: "IX_Drafts_ScratchId",
                table: "Drafts",
                column: "ScratchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Drafts_DraftId",
                table: "Questions",
                column: "DraftId",
                principalTable: "Drafts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Drafts_DraftId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "Drafts");

            migrationBuilder.DropIndex(
                name: "IX_Questions_DraftId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "DraftId",
                table: "Questions");

            migrationBuilder.AddColumn<List<string>>(
                name: "Drafts",
                table: "Scratches",
                type: "text[]",
                nullable: true);
        }
    }
}
