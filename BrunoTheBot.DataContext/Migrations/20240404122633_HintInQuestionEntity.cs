using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class HintInQuestionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Topics_TopicId",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "Authors",
                newName: "ContentId");

            migrationBuilder.RenameIndex(
                name: "IX_Authors_TopicId",
                table: "Authors",
                newName: "IX_Authors_ContentId");

            migrationBuilder.AddColumn<string>(
                name: "Hint",
                table: "Questions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Contents_ContentId",
                table: "Authors",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Contents_ContentId",
                table: "Authors");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropColumn(
                name: "Hint",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "ContentId",
                table: "Authors",
                newName: "TopicId");

            migrationBuilder.RenameIndex(
                name: "IX_Authors_ContentId",
                table: "Authors",
                newName: "IX_Authors_TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Topics_TopicId",
                table: "Authors",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id");
        }
    }
}
