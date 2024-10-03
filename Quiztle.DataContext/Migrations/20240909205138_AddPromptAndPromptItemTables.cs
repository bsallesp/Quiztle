using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiztle.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class AddPromptAndPromptItemTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "Prompts");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Prompts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Sentence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sentence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromptItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PromptId = table.Column<Guid>(type: "uuid", nullable: false),
                    SentenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    DraftId = table.Column<Guid>(type: "uuid", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromptItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromptItems_Drafts_DraftId",
                        column: x => x.DraftId,
                        principalTable: "Drafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromptItems_Prompts_PromptId",
                        column: x => x.PromptId,
                        principalTable: "Prompts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromptItems_Sentence_SentenceId",
                        column: x => x.SentenceId,
                        principalTable: "Sentence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromptItems_DraftId",
                table: "PromptItems",
                column: "DraftId");

            migrationBuilder.CreateIndex(
                name: "IX_PromptItems_PromptId_Order",
                table: "PromptItems",
                columns: new[] { "PromptId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromptItems_SentenceId",
                table: "PromptItems",
                column: "SentenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromptItems");

            migrationBuilder.DropTable(
                name: "Sentence");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Prompts");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Prompts",
                type: "text",
                nullable: true);
        }
    }
}
