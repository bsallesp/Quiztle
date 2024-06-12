using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiztle.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class ResponseIdInShots2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shots_Responses_ResponseId",
                table: "Shots");

            migrationBuilder.AlterColumn<Guid>(
                name: "ResponseId",
                table: "Shots",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shots_Responses_ResponseId",
                table: "Shots",
                column: "ResponseId",
                principalTable: "Responses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shots_Responses_ResponseId",
                table: "Shots");

            migrationBuilder.AlterColumn<Guid>(
                name: "ResponseId",
                table: "Shots",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Shots_Responses_ResponseId",
                table: "Shots",
                column: "ResponseId",
                principalTable: "Responses",
                principalColumn: "Id");
        }
    }
}
