using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiztle.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class ScratchesTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Scratches",
                newName: "Name");

            migrationBuilder.AddColumn<List<string>>(
                name: "Drafts",
                table: "Scratches",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Drafts",
                table: "Scratches");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Scratches",
                newName: "Content");
        }
    }
}
