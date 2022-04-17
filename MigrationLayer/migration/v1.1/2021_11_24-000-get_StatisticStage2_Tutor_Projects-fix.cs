using System;
using System.Net.Mime;
using FluentMigrator;
using Migrations.@abstract;

namespace Migrations
{ 
    [Migration(1_1_2021_11_24__000, "get_StatisticStage2_Tutor_Projects fix")]
    public class v1_1_2021_11_24__000 : SQLMigration
    {
        public override void Up()
        {
            ExecuteScript("/v1.1/2021_11_24-000-alter-get_StatisticStage2_Tutor_Projects-fix.sql");
        }

        public override void Down()
        {
            ExecuteScript("/v1.1/2021_11_24-000-drop-get_StatisticStage2_Tutor_Projects-fix.sql");
        }
    }                 
}