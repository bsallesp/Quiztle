using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class ResponseListShots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Shots_ShotId",
                table: "Responses");

            migrationBuilder.DropIndex(
                name: "IX_Responses_ShotId",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "ShotId",
                table: "Responses");

            migrationBuilder.AddColumn<Guid>(
                name: "ResponseId",
                table: "Shots",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "Responses",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Score",
                table: "Responses",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Shots_ResponseId",
                table: "Shots",
                column: "ResponseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shots_Responses_ResponseId",
                table: "Shots",
                column: "ResponseId",
                principalTable: "Responses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shots_Responses_ResponseId",
                table: "Shots");

            migrationBuilder.DropIndex(
                name: "IX_Shots_ResponseId",
                table: "Shots");

            migrationBuilder.DropColumn(
                name: "ResponseId",
                table: "Shots");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Responses");

            migrationBuilder.AddColumn<Guid>(
                name: "ShotId",
                table: "Responses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Responses_ShotId",
                table: "Responses",
                column: "ShotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Shots_ShotId",
                table: "Responses",
                column: "ShotId",
                principalTable: "Shots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
