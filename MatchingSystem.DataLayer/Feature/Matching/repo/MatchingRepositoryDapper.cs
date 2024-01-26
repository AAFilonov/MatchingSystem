using Dapper;
using MatchingSystem.DataLayer.Utils;

namespace MatchingSystem.DataLayer.Feature.Matching.repo;

public class MatchingRepositoryDapper : ConnectionBase,IMatchingRepositoryDapper
{
    public MatchingRepositoryDapper(string connectionString) : base(connectionString)
    {
    }

    //TODO DPL-34 условные тесты с реальной базой на SQLite
    public void SetNextStage(int matchingId)
    {
        Connection.Query("exec napp.goto_NextStage @MatchingID",
            new {MatchingID = matchingId});
    }
}