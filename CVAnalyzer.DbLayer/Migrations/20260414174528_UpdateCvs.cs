using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVAnalyzer.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCvs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Analyses_CvId",
                table: "Analyses");

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_CvId",
                table: "Analyses",
                column: "CvId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Analyses_CvId",
                table: "Analyses");

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_CvId",
                table: "Analyses",
                column: "CvId",
                unique: true);
        }
    }
}
