#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace MatchingSystem.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NameAbbreviation",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "[Name] + ' ' + [Patronimic]+ ' ' + [Surname]",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComputedColumnSql: "[Name] + ' ' + [Patronimic]+ ' ' = [Surname]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NameAbbreviation",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "[Name] + ' ' + [Patronimic]+ ' ' = [Surname]",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComputedColumnSql: "[Name] + ' ' + [Patronimic]+ ' ' + [Surname]");
        }
    }
}
