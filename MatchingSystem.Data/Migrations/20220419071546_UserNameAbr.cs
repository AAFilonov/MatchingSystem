#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace MatchingSystem.Data.Migrations
{
    public partial class UserNameAbr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MatchingTypeCode",
                table: "MatchingType",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAbbreviation",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "[Name] + ' ' + [Patronimic]+ ' ' + [Surname]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAbbreviation",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "MatchingTypeCode",
                table: "MatchingType",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
