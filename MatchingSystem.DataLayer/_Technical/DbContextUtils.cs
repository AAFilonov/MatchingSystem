using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer._Technical;

public class DbContextUtils
{
    private static string _connectionString;

    public static void Init(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public static DbContextOptionsBuilder<DiplomaMatchingContext> CreateDbContextOptions()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DiplomaMatchingContext>();
        optionsBuilder.UseSqlServer(_connectionString);
        optionsBuilder.UseLazyLoadingProxies();
        return optionsBuilder;
    }
}