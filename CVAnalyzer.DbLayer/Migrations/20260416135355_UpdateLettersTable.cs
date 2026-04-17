using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVAnalyzer.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLettersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Users_UserId",
                table: "Letters");

            migrationBuilder.DropIndex(
                name: "IX_Letters_UserId",
                table: "Letters");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Letters",
                newName: "AnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_Letters_AnalysisId",
                table: "Letters",
                column: "AnalysisId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Analyses_AnalysisId",
                table: "Letters",
                column: "AnalysisId",
                principalTable: "Analyses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Letters_Analyses_AnalysisId",
                table: "Letters");

            migrationBuilder.DropIndex(
                name: "IX_Letters_AnalysisId",
                table: "Letters");

            migrationBuilder.RenameColumn(
                name: "AnalysisId",
                table: "Letters",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Letters_UserId",
                table: "Letters",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_Users_UserId",
                table: "Letters",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
