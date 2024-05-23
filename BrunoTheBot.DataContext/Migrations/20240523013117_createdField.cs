using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class createdField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "PDFDataPages");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "PDFDataPages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "PDFData",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "PDFDataPages");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "PDFData");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PDFDataPages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
