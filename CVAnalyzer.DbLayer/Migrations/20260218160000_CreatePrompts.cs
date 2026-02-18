using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CVAnalyzer.DbLayer.Migrations
{
    [DbContext(typeof(CVAnalyzerContext))]
    [Migration("20260218160000_CreatePrompts")]
    public class CreatePrompts: Migration
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
                        new Guid("B2F6F4E7-9E5A-4B42-9E7B-123456789001"),
                        "CvAnalysis",
                        "Проанализируй резюме ниже на недостатки. Не меняй название критериев в ответе. Раздели анализ по следующим критериям:\n " +
                        "1.Структура (досаточное ли количество разделов и их правильность, но не включай раздел контакты, считай, что он заполнен),\n" +
                        "2.Техническая составляющая(укажи, что следует изучить),\n" +
                        "3.Релевантность (проанализируй опыт, хорошо ли он прописан, как можно улучшить раздел, может быть пройти дополнительные курсы),\n" +
                        "4.Прочие рекомендации(здесь укажи всё, что ещё считаешь нужным) : Резюме:\n{0}",
                        true,
                        new DateOnly(2026, 2, 18)
                    }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}