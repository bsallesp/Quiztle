using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class ShotTableRemoveOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shots_Options_OptionId",
                table: "Shots");

            migrationBuilder.DropIndex(
                name: "IX_Shots_OptionId",
                table: "Shots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Shots_OptionId",
                table: "Shots",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shots_Options_OptionId",
                table: "Shots",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
