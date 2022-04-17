using System;
using FluentMigrator;
using Migrations.@abstract;

namespace Migrations
{
    [Migration(1_0_2021_11_21__000 , "v1.0 Baseline")]
    public class v1_2021_11_01_000_Baseline : SQLMigration
    {
        public override void Up()
        {
            //ExecuteScript("/v1.0/2021_11_21-000-Baseline.sql");  // - хранимки начинают возвращать урезанные значения - nvarchar max  изменился?
            //пока что: руками поднять базу из бэкапа DiplomaMatching_2021-11-20-17-51.bak
        }

        public override void Down()
        {
            //..Schema("dbo");
        }
    }
}