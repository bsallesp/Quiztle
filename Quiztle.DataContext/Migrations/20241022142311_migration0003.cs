using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiztle.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class migration0003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShieldSVG",
                table: "Tests",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShieldSVG",
                table: "Tests");
        }
    }
}
