using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class ShotTableAndOtherFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Responses_ResponseId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_ResponseId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ResponseId",
                table: "Questions");

            migrationBuilder.AddColumn<Guid>(
                name: "ShotId",
                table: "Responses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "OptionsDTO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shots_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Responses_ShotId",
                table: "Responses",
                column: "ShotId");

            migrationBuilder.CreateIndex(
                name: "IX_Shots_OptionId",
                table: "OptionsDTO",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Shots_ShotId",
                table: "Responses",
                column: "ShotId",
                principalTable: "OptionsDTO",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Shots_ShotId",
                table: "Responses");

            migrationBuilder.DropTable(
                name: "OptionsDTO");

            migrationBuilder.DropIndex(
                name: "IX_Responses_ShotId",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "ShotId",
                table: "Responses");

            migrationBuilder.AddColumn<Guid>(
                name: "ResponseId",
                table: "Questions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ResponseId",
                table: "Questions",
                column: "ResponseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Responses_ResponseId",
                table: "Questions",
                column: "ResponseId",
                principalTable: "Responses",
                principalColumn: "Id");
        }
    }
}
