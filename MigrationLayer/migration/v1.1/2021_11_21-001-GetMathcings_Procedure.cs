using System;
using System.Net.Mime;
using FluentMigrator;
using Migrations.@abstract;

namespace Migrations
{ 
    [Migration(1_1_2021_11_21__001, "v1.1 GetMatchings Procedure add")]
    public class v1_1_001 : SQLMigration
    {
        public override void Up()
        {
            ExecuteScript("/v1.1/2021_11_21-001-create-GetMatchings_Procedure.sql");
        }

        public override void Down()
        {
            ExecuteScript("/v1.1/2021_11_21-001-drop-GetMatchings_Procedure.sql");
        }
    }                 
}