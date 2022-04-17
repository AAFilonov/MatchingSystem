using System;
using System.Net.Mime;
using FluentMigrator;
using Migrations.@abstract;

namespace Migrations
{
    [Migration(1_1_2021_11_21__000, "v1.1 Magisters 2021")]
    public class v1_1_000 : SQLMigration
    {
        public override void Up()
        {
            ExecuteScript("/v1.1/2021_11_21-000-Magisters2021.sql");
        }

        public override void Down()
        {
        }
    }
}