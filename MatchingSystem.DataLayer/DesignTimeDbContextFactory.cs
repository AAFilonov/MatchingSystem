using System;
using System.IO;
using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace MatchingSystem.DataLayer;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DiplomaMatchingContext>
{
    public DiplomaMatchingContext CreateDbContext(string[] args)
    {
        DirectoryInfo environmentDir = new DirectoryInfo(Environment.CurrentDirectory);
        Console.WriteLine("Reading config from {0}", environmentDir.FullName);
        var builder = new ConfigurationBuilder()
            .SetBasePath(environmentDir.FullName)
            .AddJsonFile($"appsettings.Data.json", optional: true);
        IConfiguration configuration = builder.Build();
        SystemParameters.Init(configuration);
        
        var optionsBuilder = new DbContextOptionsBuilder<DiplomaMatchingContext>();
        optionsBuilder.UseSqlServer(SystemParameters.getInstance().CONNECTION_STRING);
        return new DiplomaMatchingContext(optionsBuilder.Options);
    }
}