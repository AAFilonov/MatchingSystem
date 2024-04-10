#nullable disable


using Common;
using Common.Utils;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MatchingSystem.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddGoToNextStageStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (SystemParameters.getInstance()?.DB_TYPE==DbType.SqlServer)
            {
                var filename = "../MatchingSystem.DataLayer/Migrations/sql/mssql/AddGoToNextStageStoredProcedures.sql";
                var sql = IOUtils.ReadFileAsString(filename);   
                migrationBuilder.Sql(sql);
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
