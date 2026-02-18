using CVAnalyzer.DbLayer.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CVAnalyzer.DbLayer.Migrations
{
    [DbContext(typeof(CVAnalyzerContext))]
    [Migration("20251205160000_CreateUser")]
    public class CreateUser: Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FirstName", "LastName" },
                columnTypes: new string[] { "uniqueidentifier", "nvarchar(max)", "nvarchar(max)" },
                values: new object[,]
                {
                    { Guid.Parse("7f679a99-6b3e-4007-8b8b-5d86bfa73902"), "User name", "User last name", }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}