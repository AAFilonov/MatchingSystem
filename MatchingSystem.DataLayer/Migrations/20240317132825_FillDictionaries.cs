using MatchingSystem.Constants;
using MatchingSystem.Constants.Utils;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchingSystem.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class FillDictionaries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string filename ="";
            if (SystemParameters.getInstance().DB_TYPE==DbType.SqlServer)
            {
                filename = "../MatchingSystem.DataLayer/Migrations/sql/fillDictionariesMsSQL.sql";
            }
            if (SystemParameters.getInstance().DB_TYPE==DbType.SqliteMemory ||SystemParameters.getInstance().DB_TYPE==DbType.SqliteFile )
            {
                filename = "../MatchingSystem.DataLayer/Migrations/sql/fillDictionariesSqlite.sql";
            }
       
           var sql = IOUtils.ReadFileAsString(filename);
           migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
     
        }
    }
}
