using System;
using System.Linq;
using MatchingSystem.Constants;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Utils;

public class DbContextFactory
{
    private static DiplomaMatchingContext? _inMemoryDiplomaMatchingContext;
    private static SqliteConnection? _dbConnection;
    private static bool _isFirstRun = true;

    public static void Dispose()
    {
        if (_dbConnection != null) _dbConnection.Close();
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
            case DbType.InMemory:
                return CreateInMemoryContext();
            default:
                throw new Exception("Некорректный тип базы данных: " + type);
        }
    }

    private static DiplomaMatchingContext CreateInMemoryContext()
    {
        if (_inMemoryDiplomaMatchingContext == null)
        {
            var options = new DbContextOptionsBuilder<DiplomaMatchingContext>()
                .UseInMemoryDatabase(databaseName: "DiplomaMatching")
                .Options;
            _inMemoryDiplomaMatchingContext = new DiplomaMatchingContext(options);
            if (SystemParameters.getInstance().USE_MIGRATIONS) ApplyMigrations(_inMemoryDiplomaMatchingContext);
        }

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

        var options = new DbContextOptionsBuilder<DiplomaMatchingContext>()
            .UseSqlite(_dbConnection!)
            .Options;

        var context = new DiplomaMatchingContext(options);
        if (firstRun && SystemParameters.getInstance().USE_MIGRATIONS) ApplyMigrations(context);

        return context;
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