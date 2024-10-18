using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiztle.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class migration0025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Consistency",
                table: "Questions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Consistency",
                table: "Questions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
