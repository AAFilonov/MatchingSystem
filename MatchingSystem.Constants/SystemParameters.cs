

using System;
using Microsoft.Extensions.Configuration;

namespace MatchingSystem.Constants;

public enum DbType
{
    SqlServer,
    SqliteMemory,
    SqliteFile,
}

public class SystemParameters
{
    private static SystemParameters _instance;

    public static SystemParameters getInstance()
    {
        return _instance;
    }

    public string CONNECTION_STRING { get; }
    public bool USE_MIGRATIONS { get;  }
    public DbType DB_TYPE { get;  }
    
    private SystemParameters(IConfiguration configuration)
    {
        CONNECTION_STRING = configuration.GetConnectionString("DefaultConnection");
        USE_MIGRATIONS = Boolean.Parse( configuration.GetSection("ApplicationSettings")["UseMigrations"] ?? "false");
        DB_TYPE = Enum.Parse<DbType>( configuration.GetSection("ApplicationSettings")["DbType"]?? throw new Exception("Не получилось определить тип СУБД для подключения."));
    }
    
  
    public static void Init(IConfiguration configuration)
    {
        _instance = new SystemParameters(configuration);
    }
}