using System;
using MatchingSystem.Constants;
using MatchingSystem.DataLayer._Technical;
using Microsoft.Extensions.Configuration;

namespace MatchingSystem.Service.Tests.Utils;

public class TestUtils
{
    public static void InitDb()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile($"appsettings.Test.json", optional: true);
        IConfiguration configuration = builder.Build();
        SystemParameters.Init(configuration);
    }
    public static void DisposeDb()
    {
        DbContextUtils.Dispose();
    }
}