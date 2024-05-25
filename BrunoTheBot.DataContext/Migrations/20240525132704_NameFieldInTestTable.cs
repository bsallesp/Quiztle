using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class NameFieldInTestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_PDFData_PDFDataId",
                table: "Tests");

            migrationBuilder.AlterColumn<Guid>(
                name: "PDFDataId",
                table: "Tests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tests",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_PDFData_PDFDataId",
                table: "Tests",
                column: "PDFDataId",
                principalTable: "PDFData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_PDFData_PDFDataId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Tests");

            migrationBuilder.AlterColumn<Guid>(
                name: "PDFDataId",
                table: "Tests",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_PDFData_PDFDataId",
                table: "Tests",
                column: "PDFDataId",
                principalTable: "PDFData",
                principalColumn: "Id");
        }
    }
}
