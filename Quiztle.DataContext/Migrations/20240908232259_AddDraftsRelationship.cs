using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiztle.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class AddDraftsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drafts_Scratches_ScratchId",
                table: "Drafts");

            migrationBuilder.AddForeignKey(
                name: "FK_Drafts_Scratches_ScratchId",
                table: "Drafts",
                column: "ScratchId",
                principalTable: "Scratches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drafts_Scratches_ScratchId",
                table: "Drafts");

            migrationBuilder.AddForeignKey(
                name: "FK_Drafts_Scratches_ScratchId",
                table: "Drafts",
                column: "ScratchId",
                principalTable: "Scratches",
                principalColumn: "Id");
        }
    }
}
