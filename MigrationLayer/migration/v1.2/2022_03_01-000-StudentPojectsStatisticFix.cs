using System;
using System.Net.Mime;
using FluentMigrator;
using Migrations.@abstract;

namespace Migrations
{
    [Migration(1_2_2022_03_01__000, "v1.2 Student project Statistic fix")]
    public class v1_2_000 : SQLMigration
    {
        public override void Up()
        {
            ExecuteScript("/v1.2/2022_03_01-000-StudentPojectsStatisticFix.sql");
        }

        public override void Down()
        {
        }
    }
}