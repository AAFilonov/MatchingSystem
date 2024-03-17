using MatchingSystem.Constants;
using MatchingSystem.Constants.Utils;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchingSystem.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddGoToNextStageStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (SystemParameters.getInstance().DB_TYPE==DbType.SqlServer)
            {
                var filename = "../MatchingSystem.DataLayer/Migrations/sql/AddGoToNextStageStoredProcedures.sql";
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
