using System;
using MatchingSystem.Constants;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer._Technical;
using Microsoft.Extensions.Configuration;

namespace MatchingSystem.Service.Tests.Utils;

public class TestUtils
{
    public static DiplomaMatchingContext InitDb(string settingsPath = "appsettings.Test.json")
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(settingsPath, optional: true);
        IConfiguration configuration = builder.Build();
        SystemParameters.Init(configuration);
        return DbContextUtils.GetContext();
    }
    public static void DisposeDb()
    {
        DbContextFactory.Dispose();
    }
}