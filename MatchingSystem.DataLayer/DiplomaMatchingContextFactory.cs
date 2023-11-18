using System;
using System.Diagnostics;
using MatchingSystem.DataLayer._Technical;
using MatchingSystem.DataLayer.DbSchema.Tables;
using MatchingSystem.DataLayer.DbSchema.View;
using MatchingSystem.DataLayer.Feature._LinkTables;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.Group;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Role;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.TutorChoise;
using MatchingSystem.DataLayer.Feature.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MatchingSystem.DataLayer;

public class DiplomaMatchingContextFactory : IDesignTimeDbContextFactory<DiplomaMatchingContext>
{
    public DiplomaMatchingContext CreateDbContext(string[] args)
    {
        //Debugger.Launch();
        var connectionString = args[0];

        var optionsBuilder = new DbContextOptionsBuilder<DiplomaMatchingContext>();
        optionsBuilder.UseSqlServer(connectionString, b =>
        {
            //b.MigrationsAssembly("MatchingSystem.DataLayer.Migrations"); 
        });
        optionsBuilder.UseLazyLoadingProxies();
        

        return new DiplomaMatchingContext(optionsBuilder.Options);
    }
}