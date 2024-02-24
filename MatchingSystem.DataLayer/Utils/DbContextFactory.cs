using System;
using System.Linq;
using MatchingSystem.Constants;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MatchingSystem.DataLayer.Utils;

public class DbContextFactory
{
    private static DiplomaMatchingContext? _inMemoryDiplomaMatchingContext;
    private static SqliteConnection? _dbConnection;
    private static bool _isFirstRun = true;

    public static void Dispose()
    {
        switch(SystemParameters.getInstance().DB_TYPE)
        {
            case DbType.SqliteMemory:
                disposeSqlLiteInMemory();
                break;
            case DbType.EFInMemory:
                disposeEfInMemory();
                break;
            case DbType.SqlServer:
            case DbType.SqliteFile:
                break;
        }
    }
    private static void disposeSqlLiteInMemory()
    {
        _inMemoryDiplomaMatchingContext?.Database.EnsureDeleted();
        _dbConnection?.Close();
        _inMemoryDiplomaMatchingContext.Dispose();
        _inMemoryDiplomaMatchingContext = null;
    }

    private static void disposeEfInMemory()
    {
        _inMemoryDiplomaMatchingContext?.Database.EnsureDeleted();
        _inMemoryDiplomaMatchingContext = null;
    }

    public static DiplomaMatchingContext GetContext()
    {
        return GetContext(SystemParameters.getInstance().DB_TYPE);
    }
    
    public static DiplomaMatchingContext GetContext(DbType type)
    {
        switch (type)
        {
            case DbType.SqlServer:
                return CreateSqlServerContext();
            case DbType.SqliteFile:
            case DbType.SqliteMemory:
                return CreateSqliteContext();
            case DbType.EFInMemory:
                return CreateInMemoryContext();
            default:
                throw new Exception("Некорректный тип базы данных: " + type);
        }
    }

    private static DiplomaMatchingContext CreateInMemoryContext()
    {
        if (_inMemoryDiplomaMatchingContext != null) return _inMemoryDiplomaMatchingContext;
        
        var options = new DbContextOptionsBuilder<DiplomaMatchingContext>()
            .UseInMemoryDatabase(databaseName: "DiplomaMatching")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        _inMemoryDiplomaMatchingContext = new DiplomaMatchingContext(options);
        if (SystemParameters.getInstance().USE_MIGRATIONS) ApplyMigrations(_inMemoryDiplomaMatchingContext);

        return _inMemoryDiplomaMatchingContext;
    }

    private static DiplomaMatchingContext CreateSqliteContext()
    {
        var firstRun = _dbConnection == null;
        if (firstRun)
        {
            _dbConnection = new SqliteConnection(SystemParameters.getInstance().CONNECTION_STRING);
            _dbConnection.Open();
            _dbConnection.CreateFunction("getdate", () => DateTime.Now);
        }

        if (_inMemoryDiplomaMatchingContext == null)
        {
            var options = new DbContextOptionsBuilder<DiplomaMatchingContext>()
                .UseSqlite(_dbConnection!)
                .EnableSensitiveDataLogging()
                .Options;

            _inMemoryDiplomaMatchingContext = new DiplomaMatchingContext(options);
        }

        if (firstRun && SystemParameters.getInstance().USE_MIGRATIONS) ApplyMigrations(_inMemoryDiplomaMatchingContext);

        return _inMemoryDiplomaMatchingContext;
    }

    private static DiplomaMatchingContext CreateSqlServerContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DiplomaMatchingContext>();
        optionsBuilder.UseSqlServer(SystemParameters.getInstance().CONNECTION_STRING);
        optionsBuilder.UseLazyLoadingProxies();
        var context = new DiplomaMatchingContext(optionsBuilder.Options);
        if (_isFirstRun && SystemParameters.getInstance().USE_MIGRATIONS) ApplyMigrations(context);
        _isFirstRun = false;

        return context;
    }

    private static void ApplyMigrations(DiplomaMatchingContext dbcontext)
    {
        var pendingMigrations = dbcontext.Database.GetPendingMigrations().ToList();
        Console.WriteLine("Applying pending migrations: ");
        foreach (var migration in pendingMigrations) Console.WriteLine(migration);
        dbcontext.Database.Migrate();
    }
}