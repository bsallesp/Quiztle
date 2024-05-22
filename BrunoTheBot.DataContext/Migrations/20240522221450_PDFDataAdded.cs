using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class PDFDataAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PDFData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDFData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDFDataPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Page = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    PDFDataId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDFDataPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PDFDataPages_PDFData_PDFDataId",
                        column: x => x.PDFDataId,
                        principalTable: "PDFData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PDFDataPages_PDFDataId",
                table: "PDFDataPages",
                column: "PDFDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PDFDataPages");

            migrationBuilder.DropTable(
                name: "PDFData");
        }
    }
}
