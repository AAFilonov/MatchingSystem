using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Data.Feature.Log;

public class LogRepository : Repository<Model.Log>, ILogRepository
{
    public LogRepository(DbContext context) : base(context)
    {
    }
}