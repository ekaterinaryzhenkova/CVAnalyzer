using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVAnalyzer.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAnalysisTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VacancyText",
                table: "Analyses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VacancyText",
                table: "Analyses");
        }
    }
}
