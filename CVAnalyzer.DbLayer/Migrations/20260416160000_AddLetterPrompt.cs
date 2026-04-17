using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CVAnalyzer.DbLayer.Migrations
{
    [DbContext(typeof(CVAnalyzerContext))]
    [Migration("20260416160000_CreatePrompts")]
    public class AddLetterPrompt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Prompts",
                columns: new[] { "Id", "Name", "Content", "IsActive", "CreatedAt" },
                columnTypes: new string[] { "uniqueidentifier", "nvarchar(max)", "nvarchar(max)", "bit", "date" },
                values: new object[,]
                {
                    {
                        new Guid("DECE4D07-9943-484E-B7C2-F8B223EFFF8F"),
                        "LetterCreating",
                        "Мое резюме:\n " +
                        "{0}\n" +
                        "Требования к вакансии, на которую я хочу подать свое резюме:\n" +
                        "{1}\n" +
                        "Сгенерируй сопроводительное письмо",
                        true,
                        new DateOnly(2026, 4, 16)
                    }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}