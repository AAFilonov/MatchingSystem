using System;
using MatchingSystem.Constants;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MatchingSystem.Service.Tests.Utils;

public class TestUtils
{
    public static DiplomaMatchingContext InitDb(string settingsPath = "appsettings.Test.json")
    {
        InitConfiguration(settingsPath);
        return DbContextFactory.GetContext();
    }
    public static IConfiguration InitConfiguration(string settingsPath = "appsettings.Test.json")
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(settingsPath, optional: true);
        IConfiguration configuration = builder.Build();
        SystemParameters.Init(configuration);
        return configuration;
    }
    
    public static void DisposeDb()
    {
        DbContextFactory.Dispose();
    }

    public static void DetachAllEntities(DiplomaMatchingContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries())
        {
            entry.State = EntityState.Detached;
        }
    }

    public static void initDictionaryTables(DiplomaMatchingContext context)
    {
        context.MatchingTypes.Add(new MatchingType()
            { MatchingTypeId = 1, MatchingTypeCode = "BA", MatchingTypeName = "Bachelors" });
        context.SaveChanges();
        context.StagesTypes.Add(new StagesType()
        {
            StageTypeId = (int)StageTypeEnum.MATCHING_INITIALIZATION,
            StageTypeName = StageTypeEnum.MATCHING_INITIALIZATION.ToString()
        });
        context.StagesTypes.Add(new StagesType()
        {
            StageTypeId = (int)StageTypeEnum.MATCHING_PREPARATION,
            StageTypeName = StageTypeEnum.MATCHING_PREPARATION.ToString()
        });
        context.StagesTypes.Add(new StagesType()
        {
            StageTypeId = (int)StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES,
            StageTypeName = StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES.ToString()
        });
        context.StagesTypes.Add(new StagesType()
            { StageTypeId = (int)StageTypeEnum.ITERATION, StageTypeName = StageTypeEnum.ITERATION.ToString() });
        context.StagesTypes.Add(new StagesType()
        {
            StageTypeId = (int)StageTypeEnum.MANUAL_ADJUSTMENT,
            StageTypeName = StageTypeEnum.MANUAL_ADJUSTMENT.ToString()
        });
        context.StagesTypes.Add(new StagesType()
            { StageTypeId = (int)StageTypeEnum.FINAL, StageTypeName = StageTypeEnum.FINAL.ToString() });

        context.SaveChanges();
    }
}