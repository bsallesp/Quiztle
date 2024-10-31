using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiztle.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class migration0015 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TestId",
                table: "Paids",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestId",
                table: "Paids");
        }
    }
}
