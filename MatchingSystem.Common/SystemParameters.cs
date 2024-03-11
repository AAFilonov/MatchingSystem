using System;
using Microsoft.Extensions.Configuration;

namespace MatchingSystem.Constants;

public enum DbType
{
    SqlServer,
    SqliteMemory,
    SqliteFile,
    EFInMemory,
}

public class SystemParameters
{
    private static SystemParameters _instance;

    public static SystemParameters getInstance()
    {
        return _instance;
    }

    public string CONNECTION_STRING { get; }
    public string VERSION { get; }
    public bool USE_MIGRATIONS { get; }
    public DbType DB_TYPE { get; }
    
    public string MAIL_HOST { get; }
    public int MAIL_PORT { get; }
    public Boolean MAIL_USE_SSL { get; }
    public string MAIL_USERNAME { get; }
    public string MAIL_PASSWORD { get; }
    public string MAIL_FROM_ADRESS { get; }
    public string MAIL_FROM_NAME { get; }

    private SystemParameters(IConfiguration configuration)
    {
        CONNECTION_STRING = configuration.GetConnectionString("DefaultConnection");
        USE_MIGRATIONS = Boolean.Parse(configuration.GetSection("ApplicationSettings:UseMigrations").Value ?? "false");
        VERSION = configuration.GetSection("ApplicationSettings:Version").Value ?? "No version info";
        DB_TYPE = Enum.Parse<DbType>(configuration.GetSection("ApplicationSettings:DbType").Value ??
                                     throw new Exception("Не получилось определить тип СУБД для подключения."));
        MAIL_HOST = configuration.GetSection("ApplicationSettings:Mail:Host").Value?? "localhost";
        MAIL_PORT = Int32.Parse(configuration.GetSection("ApplicationSettings:Mail:Port").Value ?? "25");
        MAIL_USE_SSL = Boolean.Parse(configuration.GetSection("ApplicationSettings:Mail:UseSSL").Value ?? "false");
        MAIL_FROM_ADRESS = configuration.GetSection("ApplicationSettings:Mail:From").Value;
        MAIL_FROM_NAME = configuration.GetSection("ApplicationSettings:Mail:FromName").Value;
        MAIL_USERNAME = configuration.GetSection("ApplicationSettings:Mail:User").Value;
        MAIL_PASSWORD = configuration.GetSection("ApplicationSettings:Mail:Password").Value;
    }


    public static void Init(IConfiguration configuration)
    {
        _instance = new SystemParameters(configuration);
    }
}